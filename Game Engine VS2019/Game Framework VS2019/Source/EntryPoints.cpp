#include "stdafx.h"

extern "C" __declspec(dllexport) void PrintMessage(LPCTSTR message)
{
	MessageBox(nullptr, message, L"Message from Game Framework", MB_OK);
}