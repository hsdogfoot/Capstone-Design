#pragma once

#include <Windows.h>

#include <cstdint>
#include <memory>

#include <string>
#include <vector>
#include <unordered_map>
#include <algorithm>

#pragma comment(lib, "d3d11.lib")
#pragma comment(lib, "D3DCompiler.lib")
#pragma comment(lib, "DirectXTK.lib")
#pragma comment(lib, "DXGI.lib")
#pragma comment(lib, "dinput8.lib")
#pragma comment(lib, "dxguid.lib")

#include <d3d11.h>
#include <d3dcompiler.h>
#include <DirectXMath.h>
#include <dinput.h>
#include <WICTextureLoader.h>

using namespace DirectX;

#include "ErrorLogger.h"
#include "StringConverter.h"

#include "FrameworkUtility.h"