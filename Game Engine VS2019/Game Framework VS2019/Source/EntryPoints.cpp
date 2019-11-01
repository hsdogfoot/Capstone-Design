#include "stdafx.h"
#include "Game.h"
#include "RenderWindow.h"

Game* g_instance = nullptr;

extern "C" __declspec(dllexport) void PrintMessage(LPCTSTR message)
{
	MessageBox(nullptr, message, L"Message from Game Framework", MB_OK);
}

extern "C" __declspec(dllexport) bool CreateGame(HINSTANCE hInstance, HWND parentHandle, int windowWidth, int windowHeight)
{
	RenderWindowDesc windowDesc;
	ZeroMemory(&windowDesc, sizeof(RenderWindowDesc));

	windowDesc.HInstance = hInstance;
	windowDesc.ParentHandle = parentHandle;
	windowDesc.Title = "Game Preview";
	windowDesc.ClassName = "Game Preview";
	windowDesc.Width = windowWidth;
	windowDesc.Height = windowHeight;

	if (!Game::Create(&g_instance, windowDesc))
	{
		MessageBox(nullptr, L"Game instance create failed. Application will be shut down...", L"Message from Game Framework", MB_ICONERROR);
		return false;
	}
	else if (g_instance == nullptr)
	{
		MessageBox(nullptr, L"Game instance create failed. Application will be shut down...", L"Message from Game Framework", MB_ICONERROR);
		return false;
	}

	MessageBox(nullptr, L"Game instance create successed.", L"Message from Game Framework", MB_OK);

	return true;
}

extern "C" __declspec(dllexport) void UpdateGame()
{
	Assert(g_instance != nullptr);

	g_instance->Update();
}

extern "C" __declspec(dllexport) void RenderGame()
{
	Assert(g_instance != nullptr);

	g_instance->Render();
}

extern "C" __declspec(dllexport) void DestroyGame()
{
	ReleaseBase(g_instance);
	g_instance = nullptr;

	MessageBox(nullptr, L"Game instance destroy successed.", L"Message from Game Framework", MB_OK);
}

extern "C" __declspec(dllexport) HWND GetWindowHandle()
{
	Assert(g_instance != nullptr);

	return g_instance->GetWindowHandle();
}