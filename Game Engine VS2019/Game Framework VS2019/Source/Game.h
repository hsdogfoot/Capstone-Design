#pragma once
#include "Application.h"

struct RenderWindowDesc;

class Game final : public Application
{
public:
	static bool Create(Game** outInstance, RenderWindowDesc& renderWindowDesc);

	void Update();
	void Render();

private:
	Game();
	virtual ~Game() override = default;

	bool initialize(RenderWindowDesc& renderWindowDesc);
	bool loadComponents() const;
	bool loadResources() const;
	bool loadActors() const;
	bool loadPrefabs() const;
	bool loadCameras() const;

	virtual bool resize() override;

	virtual void destroy() override;

public:
	bool IsEditMode = false;
};