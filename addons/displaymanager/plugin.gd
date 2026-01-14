@tool
extends EditorPlugin

# Replace this value with a PascalCase autoload name, as per the GDScript style guide.
const AUTOLOAD_NAME: String = "DisplayManager"
const AUTOLOAD_SCENE_PATH: String = "res://addons/displaymanager/DisplayManager.tscn"
const INPUT_SETTING_PREFIX: String = "input/"
const INPUT_MAP_ACTION_NAME: String = "fullscreen_toggle"


func _enable_plugin():
	add_autoload_singleton(AUTOLOAD_NAME, AUTOLOAD_SCENE_PATH)
	print("%s autoload added" % AUTOLOAD_NAME)
	_add_input_action()
	print("%s input action added" % INPUT_MAP_ACTION_NAME)
	_save_settings()
	print("DisplayManager plugin enabled")


func _disable_plugin():
	remove_autoload_singleton(AUTOLOAD_NAME)
	print("%s autoload removed" % AUTOLOAD_NAME)
	_remove_input_action()
	print("%s input action removed" % INPUT_MAP_ACTION_NAME)
	_save_settings()
	print("DisplayManager plugin disabled")


func _add_input_action():
	if not ProjectSettings.has_setting(INPUT_SETTING_PREFIX + INPUT_MAP_ACTION_NAME):
		var event_key: InputEventKey = InputEventKey.new()
		event_key.physical_keycode = KEY_ENTER
		event_key.alt_pressed = true
		
		var action = {
			"deadzone": 0.5,
			"events": [event_key]
		}
		ProjectSettings.set_setting(INPUT_SETTING_PREFIX + INPUT_MAP_ACTION_NAME, action)
	else:
		print("%s input action already exists" % INPUT_MAP_ACTION_NAME)


func _remove_input_action():
	if ProjectSettings.has_setting(INPUT_SETTING_PREFIX + INPUT_MAP_ACTION_NAME):
		ProjectSettings.set_setting(INPUT_SETTING_PREFIX + INPUT_MAP_ACTION_NAME, null)


func _save_settings():
	var error: int = ProjectSettings.save()
	if error == OK:
		print("Project settings saved")
	else:
		print("Error saving project settings: ", error)
