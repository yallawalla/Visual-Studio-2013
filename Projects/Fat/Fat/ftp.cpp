#undef UNICODE

#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <thread>

#include "ff.h"
#include "ftp.h"

// Need to link with Ws2_32.lib
#pragma comment (lib, "Ws2_32.lib")
// #pragma comment (lib, "Mswsock.lib")

#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "21"


int send_msg(SOCKET s, char *msg, ...)
{
	va_list arg;
	char buffer[256];
	int len;

	va_start(arg, msg);
	vsprintf_s(buffer, msg, arg);
	va_end(arg);
	strcat_s(buffer, "\r\n");
	return	send(s, buffer, strlen(buffer), 0);
}

struct 			ftpd_command {
	char *cmd;
	void(*func) (const char *, struct ftpd_msgstate *);
};

void cmd_user(const char *arg, struct ftpd_msgstate *fsm) {
	send_msg(fsm->msg_socket, msg331);
	fsm->state = FTPD_PASS;
	//		send_msg(pcb, fs, msgLoginFailed);
	//		fs->state = FTPD_QUIT;
}

void cmd_pass(const char *arg, struct ftpd_msgstate *fsm) {
	send_msg(fsm->msg_socket, msg230);
	fsm->state = FTPD_IDLE;
	//		send_msg(pcb, fs, msgLoginFailed);
	//		fs->state = FTPD_QUIT;
}

static void cmd_syst(const char *arg, struct ftpd_msgstate *fsm)
{
	send_msg(fsm->msg_socket, msg214SYST, "UNIX");
}

static void cmd_type(const char *arg, struct ftpd_msgstate *fsm)
{
	printf("Got TYPE -%s-\n", arg);
	if (*arg == 'A')
	{
		send_msg(fsm->msg_socket, "200 Type set to A.");
		return;
	}
	if (*arg == 'I')
	{
		send_msg(fsm->msg_socket, "200 Type set to I.");
		return;
	}

	send_msg(fsm->msg_socket, msg502);
}

void cmd_pasv(const char *arg, struct ftpd_msgstate *fsm)
{
	static int port = 4096;
	static int start_port = 4096;
	struct tcp_pcb *temppcb;
	
	fsm->datafs = new struct ftpd_datastate();

	if (fsm->datafs == NULL) {
		send_msg(pcb, fsm, msg451);
		return;
	}
	memset(fsm->datafs, 0, sizeof(struct ftpd_datastate));

	fsm->datapcb = tcp_new();
	if (!fsm->datapcb) {
		mem_free(fsm->datafs);
		send_msg(pcb, fsm, msg451);
		return;
	}
	start_port = port;
	while (1) {
		err_t err;

		if (++port > 0x7fff)
			port = 4096;

		fsm->dataport = port;
		err = tcp_bind(fsm->datapcb, &pcb->local_ip, fsm->dataport);
		if (err == ERR_OK)
			break;
		if (start_port == port)
			err = ERR_CLSD;
		if (err == ERR_USE)
			continue;
		if (err != ERR_OK) {
			ftpd_dataclose(fsm->datapcb, fsm->datafs);
			fsm->datapcb = NULL;
			fsm->datafs = NULL;
			return;
		}
	}

	temppcb = tcp_listen(fsm->datapcb);
	if (!temppcb) {
		ftpd_dataclose(fsm->datapcb, fsm->datafs);
		fsm->datapcb = NULL;
		fsm->datafs = NULL;
		return;
	}
	fsm->datapcb = temppcb;

	fsm->passive = 1;
	fsm->datafs->state = _CLOSED;
	fsm->datafs->msgfs = fsm;
	fsm->datafs->msgpcb = pcb;

	/* Tell TCP that this is the structure we wish to be passed for our
	callbacks. */
	tcp_arg(fsm->datapcb, fsm->datafs);
	tcp_accept(fsm->datapcb, ftpd_dataaccept);
	send_msg(pcb, fsm, msg227, ip4_addr1(&pcb->local_ip), ip4_addr2(&pcb->local_ip), ip4_addr3(&pcb->local_ip), ip4_addr4(&pcb->local_ip), (fsm->dataport >> 8) & 0xff, (fsm->dataport) & 0xff);
	//   dbg_printf("\r\ndatapcb=%08X, dataarg=%08X\r\n", fsm->datapcb,fsm->datafs);

}
static struct ftpd_command ftpd_commands[] = {
	"USER", cmd_user,
	"PASS", cmd_pass,
	"SYST", cmd_syst,
	"TYPE", cmd_type,
	//"PORT", cmd_port,
	//"QUIT", cmd_quit,
	//"CWD", cmd_cwd,
	//"CDUP", cmd_cdup,
	//"PWD", cmd_pwd,
	//"XPWD", cmd_pwd,
	//"NLST", cmd_nlst,
	//"LIST", cmd_list,
	//"RETR", cmd_retr,
	//"STOR", cmd_stor,
	//"NOOP", cmd_noop,
	//"ABOR", cmd_abrt,
	//"MODE", cmd_mode,
	//"RNFR", cmd_rnfr,
	//"RNTO", cmd_rnto,
	//"MKD", cmd_mkd,
	//"XMKD", cmd_mkd,
	//"RMD", cmd_rmd,
	//"XRMD", cmd_rmd,
	//"DELE", cmd_dele,
	//"SIZE", cmd_size,
	"PASV", cmd_pasv,
	NULL
}; 

int __cdecl mainn(void)
{
	WSADATA wsaData;
	int iResult;
	u_long iMode = -1;

	SOCKET ListenSocket = INVALID_SOCKET;

	struct addrinfo *result = NULL;
	struct addrinfo hints;

	// Initialize Winsock
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != 0) {
		printf("WSAStartup failed with error: %d\n", iResult);
		return 1;
	}

	ZeroMemory(&hints, sizeof(hints));
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;
	hints.ai_flags = AI_PASSIVE;

	// Resolve the server address and port
	iResult = getaddrinfo(NULL, DEFAULT_PORT, &hints, &result);
	if (iResult != 0) {
		printf("getaddrinfo failed with error: %d\n", iResult);
		WSACleanup();
		return 1;
	}

	// Create a SOCKET for connecting to server
	ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
	if (ListenSocket == INVALID_SOCKET) {
		printf("socket failed with error: %ld\n", WSAGetLastError());
		freeaddrinfo(result);
		WSACleanup();
		return 1;
	}

	// Setup the TCP listening socket
	iResult = bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
	if (iResult == SOCKET_ERROR) {
		printf("bind failed with error: %d\n", WSAGetLastError());
		freeaddrinfo(result);
		closesocket(ListenSocket);
		WSACleanup();
		return 1;
	}

	freeaddrinfo(result);

	iResult = listen(ListenSocket, SOMAXCONN);
	if (iResult == SOCKET_ERROR) {
		printf("listen failed with error: %d\n", WSAGetLastError());
		closesocket(ListenSocket);
		WSACleanup();
		return 1;
	}
	DWORD WINAPI ListenForClients(LPVOID);
	DWORD myThreadID;
	HANDLE myHandle = CreateThread(0, 0, ListenForClients, &ListenSocket , 0, &myThreadID);
	while (1);
}

DWORD WINAPI ListenForClients(LPVOID lpParameter) {
	SOCKET ListenSocket = *((SOCKET *)lpParameter);

	struct ftpd_msgstate *fsm = new ftpd_msgstate();
    fsm->state = FTPD_IDLE;
//    fsm->vfs = vfs_openfs();
	while (true) // Never ends until the Server is closed.
	{
		fsm->msg_socket = accept(ListenSocket, NULL, NULL);
		DWORD WINAPI HandleClientComm(LPVOID);
		DWORD myThreadID;
		HANDLE myHandle = CreateThread(0, 0, HandleClientComm, fsm, 0, &myThreadID);
	}
	return 0;
}


DWORD WINAPI HandleClientComm(LPVOID lpParameter) {
	struct ftpd_msgstate *fsm = (struct ftpd_msgstate *)lpParameter;
	int i=send_msg(fsm->msg_socket, msg220);
	struct ftpd_command *ftpd_cmd;
	while (true) // Never ends until the Server is closed.
	{
		char recvbuf[DEFAULT_BUFLEN];
		int recvbuflen = DEFAULT_BUFLEN;
		int iResult = recv(fsm->msg_socket, recvbuf, recvbuflen, 0);
		if (iResult > 0) {
			char cmd[5];
			struct pbuf *q;
			char *pt = recvbuf;
			struct ftpd_command *ftpd_cmd;
			recvbuf[iResult] = '\0';
			printf("query: %s\n", recvbuf);
			strncpy_s(cmd, recvbuf, 4);
			for (pt = cmd; pt < &cmd[4] && isalpha(*pt); pt++)
				*pt = toupper(*pt);
			*pt = '\0';

			for (ftpd_cmd = ftpd_commands; ftpd_cmd->cmd != NULL; ftpd_cmd++) {
				if (!strcmp(ftpd_cmd->cmd, cmd))
					break;
			}

			if (strlen(recvbuf) < (strlen(cmd) + 1))
				pt = "";
			else
				pt = &recvbuf[strlen(cmd) + 1];

			if (ftpd_cmd->func)
				ftpd_cmd->func(pt,fsm);
			else
				send_msg(fsm->msg_socket, msg502);
		}
		else if (iResult == 0) {
			printf("Connection closing...\n");
			break;
		}
		else  {
			break;
		}
	}
	closesocket(fsm->msg_socket);
//	WSACleanup();
	return 0;
}

