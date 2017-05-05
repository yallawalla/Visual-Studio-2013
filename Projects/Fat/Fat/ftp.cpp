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

void list_directory(_FSM *fsm, int shortlist)
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
		Sleep(5);
	}
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
	void(*func) (const char *, _FSM *);
};

void cmd_user(const char *arg, _FSM *fsm) {
	send_msg(fsm->socket, msg331);
	fsm->mstate = FTPD_PASS;
	//		send_msg(pcb, fs, msgLoginFailed);
	//		fs->state = FTPD_QUIT;
}

void cmd_pass(const char *arg, _FSM *fsm) {
	send_msg(fsm->socket, msg230);
	fsm->mstate = FTPD_IDLE;
	//		send_msg(pcb, fs, msgLoginFailed);
	//		fs->state = FTPD_QUIT;
}

static void cmd_syst(const char *arg, _FSM *fsm)
{
	send_msg(fsm->socket, msg214SYST, "UNIX");
}

static void cmd_type(const char *arg, _FSM *fsm)
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

void cmd_pasv(const char *arg, _FSM *fsm) {
	DWORD WINAPI DataHandle(LPVOID);
	char c[8];
	_itoa_s(fsm->dataport, c, 10);

	fsm->data = Listen(c, DataHandle);
	fsm->data->data = fsm;

	if (fsm->data) {
		send_msg(fsm->socket, msg227, 127, 0, 0, 1, fsm->dataport / 256, fsm->dataport % 256);
		if (++fsm->dataport == MAX_DATAPORT)
			fsm->dataport = DEFAULT_DATAPORT;
	}
	else {
		send_msg(fsm->socket, msg451);
	}
}

void cmd_quit(const char *arg, _FSM *fsm)
{
	send_msg(fsm->socket, msg221);
	fsm->mstate = FTPD_QUIT;
}

void cmd_cwd(const char *arg, _FSM *fsm)
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

void cmd_pwd(const char *arg, _FSM *fsm)
{
	TCHAR path[256];
	if (f_getcwd(path, sizeof(path))==FR_OK)
		send_msg(fsm->socket, msg257PWD, path);	//~~~
}

 void cmd_list_common(const char *arg, _FSM *fsm, bool shortlist)
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
}

 void cmd_nlst(const char *arg, _FSM *fsm)
{
	cmd_list_common(arg, fsm, true);
}

 void cmd_list(const char *arg, _FSM *fsm)
{
	cmd_list_common(arg, fsm, false);
}

struct ftpd_command ftpd_commands[] = {
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


_FSM * Listen(PCSTR port, LPVOID lpParameter)
{
	WSADATA wsaData;
	int iResult;
	u_long iMode = -1;

	struct addrinfo *result = NULL;
	struct addrinfo hints;
	_FSM *fsm = new ftpd_msgstate();
	fsm->mstate = FTPD_IDLE;
	fsm->dataport = DEFAULT_DATAPORT;

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
	_FSM *fsm = (_FSM *)lpParameter;
	while (true) // Never ends until the Server is closed.
	{
		fsm->socket = accept(fsm->listen, NULL, NULL);
		DWORD WINAPI HandleClientComm(LPVOID);
		DWORD myThreadID;
		HANDLE myHandle = CreateThread(0, 0, fsm->MsgHandle, fsm, 0, &myThreadID);
	}
	return 0;
}

DWORD WINAPI DataHandle(LPVOID lpParameter) {
	_FSM *fsm = (_FSM *)lpParameter;
	switch (fsm->data->mstate) {
		case FTPD_LIST:
			list_directory(fsm->data, 0);
			fsm->data->mstate = FTPD_IDLE;
			closesocket(fsm->socket);
			printf("data connection closing...\n");
			break;
		case FTPD_NLST:
			list_directory(fsm->data, 1);
			fsm->data->mstate = FTPD_IDLE;
			closesocket(fsm->socket);
			break;
		case FTPD_RETR:
			//send_file(fsm->datafs, fsm->datapcb);
			break;
		default:
			break;
	}
	return 0;
}

DWORD WINAPI MsgHandle(LPVOID lpParameter) {
	_FSM *fsm = (_FSM *)lpParameter;
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
			else
				send_msg(fsm->socket, msg502);
		}
		else if (iResult == 0) {
			printf("message connection closing...\n");
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

