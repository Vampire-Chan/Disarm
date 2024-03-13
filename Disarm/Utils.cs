using GTA;
using GTA.Math;
using GTA.Native;
using Logs;
using System;

namespace Disarm
{
    public static class Utils
    {
        public static Vector3 GetCrosshairCoords(Logger logging = null)
        {
            try
            {
                logging?.Info("Getting Crosshair Coordinates!!");
                return World.Raycast(GameplayCamera.Position, GameplayCamera.Direction, 1000f, IntersectFlags.Everything, Game.Player.Character).HitPosition;
            }
            catch (Exception ex)
            {
                logging?.Error($"Error getting crosshair coordinates: {ex.Message}");
                return Vector3.Zero;
            }
        }

        public static int GetLastDamageBone(this Ped ped, Logger logging = null)
        {
            try
            {
                OutputArgument outputArgument = new OutputArgument();
                Function.Call<bool>(Hash.GET_PED_LAST_DAMAGE_BONE, new InputArgument[2] { ped, outputArgument });
                return outputArgument.GetResult<int>();
            }
            catch (Exception ex)
            {
                logging?.Error($"Error getting last damage bone: {ex.Message}");
                return -1;
            }
        }

        public static void ClearLastDamageBone(this Ped ped, Logger logging = null)
        {
            try
            {
                Function.Call(Hash.CLEAR_PED_LAST_DAMAGE_BONE, ped);
            }
            catch (Exception ex)
            {
                logging?.Error($"Error clearing last damage bone: {ex.Message}");
            }
        }

        public static void PlayAmbientSpeech(this Ped ped, string speechFile, bool immediately, Logger logging = null)
        {
            try
            {
                if (immediately)
                {
                    Function.Call(Hash.STOP_CURRENT_PLAYING_AMBIENT_SPEECH, ped);
                }
                Function.Call(Hash.SET_AUDIO_FLAG, "IsDirectorModeActive", 1);
                Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, ped, speechFile, "SPEECH_PARAMS_FORCE");
                Function.Call(Hash.SET_AUDIO_FLAG, "IsDirectorModeActive", 0);
            }
            catch (Exception ex)
            {
                logging?.Error($"Error playing ambient speech: {ex.Message}");
            }
        }

        public static bool HasBeenDamagedByWeapon(this Ped ped, WeaponHash weapon, Logger logging = null)
        {
            try
            {
                return Function.Call<bool>(Hash.HAS_PED_BEEN_DAMAGED_BY_WEAPON, new InputArgument[3]
                {
                    ped,
                    weapon.GetHashCode(),
                    0
                });
            }
            catch (Exception ex)
            {
                logging?.Error($"Error checking weapon damage: {ex.Message}");
                return false;
            }
        }

        public static void ClearLastWeaponDamage(this Ped ped, Logger logging = null)
        {
            try
            {
                Function.Call(Hash.CLEAR_PED_LAST_WEAPON_DAMAGE, ped);
            }
            catch (Exception ex)
            {
                logging?.Error($"Error clearing last weapon damage: {ex.Message}");
            }
        }
    }
}
