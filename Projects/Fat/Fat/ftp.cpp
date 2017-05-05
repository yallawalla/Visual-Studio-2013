#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <iostream>
#include <thread>

#include "ff.h"
#include "ftp.h"

// Need to link with Ws2_32.lib
#pragma comment (lib, "Ws2_32.lib")
// #pragma comment (lib, "Mswsock.lib")

void send_next_directory(struct ftpd_msgstate *fsm, int shortlist)
{
	FILINFO	fno;
	TCHAR	buf[256];
	while (1) {
		f_readdir(fsm->dir, &fno);
		if (fsm->dir->sect == NULL)
			break;
		if (shortlist)
			sprintf_s(buf, "%s\r\n", fno.fname);
		else {
			time_t current_time;
			struct tm s_time;
			time(&current_time);
			gmtime_s( &s_time, &current_time);
			sprintf_s(buf, "-rwxrwxrwx   1 user     ftp  %11d %s %02i %02i:%02i %s\r\n", fno.fsize, month_table[s_time.tm_mon], s_time.tm_mday, s_time.tm_hour, s_time.tm_min, fno.fname);
		}

		if (fno.fattrib & AM_DIR)
			buf[0] = 'd';
		send(fsm->data->socket, buf, strlen(buf), 0);
	}
	closesocket(fsm->data->socket);
}


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

struct	ftpd_command {
	char *cmd;
	void(*func) (const char *, struct ftpd_msgstate *);
};

void cmd_user(const char *arg, struct ftpd_msgstate *fsm) {
	send_msg(fsm->socket, msg331);
	fsm->mstate = FTPD_PASS;
	//		send_msg(pcb, fs, msgLoginFailed);
	//		fs->state = FTPD_QUIT;
}

void cmd_pass(const char *arg, struct ftpd_msgstate *fsm) {
	send_msg(fsm->socket, msg230);
	fsm->mstate = FTPD_IDLE;
	//		send_msg(pcb, fs, msgLoginFailed);
	//		fs->state = FTPD_QUIT;
}

static void cmd_syst(const char *arg, struct ftpd_msgstate *fsm)
{
	send_msg(fsm->socket, msg214SYST, "UNIX");
}

static void cmd_type(const char *arg, struct ftpd_msgstate *fsm)
{
	printf("Got TYPE -%s-\n", arg);
	if (*arg == 'A')
	{
		send_msg(fsm->socket, "200 Type set to A.");
		return;
	}
	if (*arg == 'I')
	{
		send_msg(fsm->socket, "200 Type set to I.");
		return;
	}

	send_msg(fsm->socket, msg502);
}

DWORD WINAPI DataHandle(LPVOID lpParameter) {
	return 0;
}

void cmd_pasv(const char *arg, struct ftpd_msgstate *fsm) {
	DWORD WINAPI DataHandle(LPVOID);
	fsm->data = Listen(NULL, DataHandle);
	if (fsm->data) {
		send_msg(fsm->socket, msg227, 127,0,0,1,0x10,0x0);
	}
	else {
		send_msg(fsm->socket, msg451);
	}
}
//{
//	DWORD WINAPI DataHandle(LPVOID);
//	static int port = 4096;
//	static int start_port = 4096;
//	char	s[8];
//	fsm->datafs = new struct ftpd_datastate();
//	start_port = port;
//	while (1) {
//		if (++port > 0x7fff)
//			port = 4096;
//
//		fsm->dataport = port;
//		sprintf(s, "%s", port);
//		if (listen(s, DataHandle) == 0)
//			break;
//		else
//			return;
//		}
//	}
//	fsm->datapcb = temppcb;
//
//	fsm->datafs->passive = 1;
//	fsm->datafs->state = _CLOSED;
//	fsm->datafs->msgfs = fsm;
//	fsm->datafs->msgpcb = pcb;
//
//	/* Tell TCP that this is the structure we wish to be passed for our
//	callbacks. */
//	tcp_arg(fsm->datapcb, fsm->datafs);
//	tcp_accept(fsm->datapcb, ftpd_dataaccept);
//	send_msg(pcb, fsm, msg227, ip4_addr1(&pcb->local_ip), ip4_addr2(&pcb->local_ip), ip4_addr3(&pcb->local_ip), ip4_addr4(&pcb->local_ip), (fsm->dataport >> 8) & 0xff, (fsm->dataport) & 0xff);
//	//   dbg_printf("\r\ndatapcb=%08X, dataarg=%08X\r\n", fsm->datapcb,fsm->datafs);
//
//}

void cmd_quit(const char *arg, struct ftpd_msgstate *fsm)
{
	send_msg(fsm->socket, msg221);
	fsm->mstate = FTPD_QUIT;
}

void cmd_cwd(const char *arg, struct ftpd_msgstate *fsm)
{
	char *c = (char *)arg;
	int i = strlen(c);
	if (i>1 && c[i - 1] == '/')
		c[i - 1] = '\0';
	while (c[i - 1] == '\r' || c[i-1] == '\n' || c[i-1] == '/')
		c[--i] = '\0';
	if (strchr(c, ':') && c[0] == '/')
		++c;	

	if (f_chdir((TCHAR *) c) == FR_OK) {
		send_msg(fsm->socket, msg250);
	}
	else {
		send_msg(fsm->socket, msg550);
	}
}

void cmd_pwd(const char *arg, struct ftpd_msgstate *fsm)
{
	TCHAR path[256];
	if (f_getcwd(path, sizeof(path))==FR_OK)
		send_msg(fsm->socket, msg257PWD, path);	//~~~
}

static void cmd_list_common(const char *arg, struct ftpd_msgstate *fsm, bool shortlist)
{
	FILINFO	fno;
	TCHAR	buf[128];

	if (f_getcwd(buf, sizeof(buf)) != FR_OK || f_opendir(fsm->dir, buf) != FR_OK || f_readdir(fsm->dir, NULL) != FR_OK) {
		send_msg(fsm->socket, msg451);
		return;
	}

	///* doesn't do anything in PASV !!! */
	//if (open_dataconnection(pcb, fsm) != 0)
	//{
	//	vfs_closedir(vfs_dir);
	//	return;
	//}

	if (shortlist) {
		fsm->mstate = FTPD_NLST;
	} else {
		fsm->mstate = FTPD_LIST;
	}

	send_msg(fsm->socket, msg150);
	send_next_directory(fsm, shortlist);
}

static void cmd_nlst(const char *arg, struct ftpd_msgstate *fsm)
{
	cmd_list_common(arg, fsm, true);
}

static void cmd_list(const char *arg, struct ftpd_msgstate *fsm)
{
	cmd_list_common(arg, fsm, false);
}

static struct ftpd_command ftpd_commands[] = {
	"USER", cmd_user,
	"PASS", cmd_pass,
	"SYST", cmd_syst,
	"TYPE", cmd_type,
	//"PORT", cmd_port,
	"QUIT", cmd_quit,
	"CWD", cmd_cwd,
	//"CDUP", cmd_cdup,
	"PWD", cmd_pwd,
	"XPWD", cmd_pwd,
	"NLST", cmd_nlst,
	"LIST", cmd_list,
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


struct ftpd_msgstate * Listen(PCSTR port, LPVOID lpParameter)
{
	WSADATA wsaData;
	int iResult;
	u_long iMode = -1;

	struct addrinfo *result = NULL;
	struct addrinfo hints;
	struct ftpd_msgstate *fsm = new ftpd_msgstate();
	fsm->mstate = FTPD_IDLE;
	fsm->port = DEFAULT_DATAPORT;
	fsm->MsgHandle = (LPTHREAD_START_ROUTINE)lpParameter;
	//    fsm->vfs = vfs_openfs();


	// Initialize Winsock
	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != 0) {
		printf("WSAStartup failed with error: %d\n", iResult);
		return NULL;
	}

	ZeroMemory(&hints, sizeof(hints));
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;
	hints.ai_flags = AI_PASSIVE;

	// Resolve the server address and port
	iResult = getaddrinfo(NULL, port, &hints, &result);
	if (iResult != 0) {
		printf("getaddrinfo failed with error: %d\n", iResult);
		WSACleanup();
		return NULL;
	}

	// Create a SOCKET for connecting to server
	fsm->listen = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
	if (fsm->listen == INVALID_SOCKET) {
		printf("socket failed with error: %ld\n", WSAGetLastError());
		freeaddrinfo(result);
		WSACleanup();
		return NULL;
	}

	// Setup the TCP listening socket
	iResult = bind(fsm->listen, result->ai_addr, (int)result->ai_addrlen);
	if (iResult == SOCKET_ERROR) {
		printf("bind failed with error: %d\n", WSAGetLastError());
		freeaddrinfo(result);
		closesocket(fsm->listen);
		WSACleanup();
		return NULL;
	}

	freeaddrinfo(result);
	iResult = listen(fsm->listen, SOMAXCONN);
	if (iResult == SOCKET_ERROR) {
		printf("listen failed with error: %d\n", WSAGetLastError());
		closesocket(fsm->listen);
		WSACleanup();
		return NULL;
	}

	DWORD WINAPI ListenForClients(LPVOID);
	DWORD myThreadID;
	HANDLE myHandle = CreateThread(0, 0, ListenForClients, fsm, 0, &myThreadID);
	return fsm;
}

DWORD WINAPI ListenForClients(LPVOID lpParameter) {
	struct ftpd_msgstate *fsm = (struct ftpd_msgstate *)lpParameter;
	while (true) // Never ends until the Server is closed.
	{
		fsm->socket = accept(fsm->listen, NULL, NULL);
		DWORD WINAPI HandleClientComm(LPVOID);
		DWORD myThreadID;
		HANDLE myHandle = CreateThread(0, 0, fsm->MsgHandle, fsm, 0, &myThreadID);
	}
	return 0;
}

DWORD WINAPI MsgHandle(LPVOID lpParameter) {
	struct ftpd_msgstate *fsm = (struct ftpd_msgstate *)lpParameter;
	int i=send_msg(fsm->socket, msg220);
	struct ftpd_command *ftpd_cmd;
	while (true) // Never ends until the Server is closed.
	{
		char recvbuf[DEFAULT_BUFLEN];
		int recvbuflen = DEFAULT_BUFLEN;
		int iResult = recv(fsm->socket, recvbuf, recvbuflen, 0);
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
			else {
				send_msg(fsm->socket, msg502);
				printf("not implemented.. %sr\n", recvbuf);
			}
		}
		else if (iResult == 0) {
			printf("Connection closing...\n");
			break;
		}
		else  {
			break;
		}
	}
	closesocket(fsm->socket);
//	WSACleanup();
	return 0;
}

