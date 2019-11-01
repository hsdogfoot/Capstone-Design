#include "stdafx.h"
#include "Game.h"

#include "GraphicDevice.h"
#include "RenderWindow.h"

bool Game::Create(Game** outInstance, RenderWindowDesc& renderWindowDesc)
{
	Assert(renderWindowDesc.HInstance != nullptr);
	Assert(renderWindowDesc.ParentHandle != nullptr);

	Game* instance = new Game();
	if (!instance->initialize(renderWindowDesc))
	{
		instance->Release();

		return false;
	}

	*outInstance = instance;

	return true;
}

void Game::Update()
{
}

void Game::Render()
{
	mGraphicDevice->RenderFrame();
}

Game::Game()
	: Application()
{
}

bool Game::initialize(RenderWindowDesc& renderWindowDesc)
{
	if (!RenderWindow::Create(&mRenderWindow, renderWindowDesc, this))
	{
		ErrorLog("RenderWindow create failed.");

		return false;
	}

	if (!GraphicDevice::Create(&mGraphicDevice, mRenderWindow->GetHandle(), mRenderWindow->GetWidth(), mRenderWindow->GetHeight()))
	{
		ErrorLog("GraphicDevice create failed.");

		return false;
	}

	if (!loadComponents())
	{
		return false;
	}

	if (!loadResources())
	{
		return false;
	}

	if (!loadActors())
	{
		return false;
	}

	if (!loadPrefabs())
	{
		return false;
	}

	if (!loadCameras())
	{
		return false;
	}
	return true;
}

bool Game::loadComponents() const
{
	return true;
}

bool Game::loadResources() const
{
	return true;
}

bool Game::loadActors() const
{
	return true;
}

bool Game::loadPrefabs() const
{
	return true;
}

bool Game::loadCameras() const
{
	return true;
}

bool Game::resize()
{
	mRenderWindow->Resize();
	if (mGraphicDevice->Resize(mRenderWindow->GetWidth(), mRenderWindow->GetHeight()))
	{
		ErrorLog("Graphic Device Resize Failed.");

		return false;
	}

	//Input, Picking ... Resize

	return true;
}

void Game::destroy()
{
	Application::destroy();
}
