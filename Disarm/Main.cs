using System;
using System.IO;
using System.Linq;
using GTA;
using Logs;


/*
namespace Disarm;

public class Main : Script
{
	public Main()
	{
		base.Tick += OnTick;
	}

	public static Model[] BlacklistedPeds = { };
	public static bool log = false;
	public static Logger logging;

	public static string[] ReadModels(string input)
	{
		string[] strings = input.Split(',');
		for (int i = 0; i < strings.Length; i++)
		{
			strings[i] = string.Join("", strings[i].Split((string[])null, StringSplitOptions.RemoveEmptyEntries));
		}
		return strings;
	}
    bool hasloaded = false;
    private void OnTick(object sender, EventArgs e)
	{

		
		Ped[] nearbyPeds = World.GetNearbyPeds(Game.Player.Character, 5000f);
		logging = new Logger("./scripts/Disarm.log");

		ScriptSettings blist = ScriptSettings.Load("./scripts/Disarm.ini");

		if (!hasloaded)
		{
			if (File.Exists("./scripts/Disarm.ini"))
				logging.Info("Disarm.ini Found. Setting Up Values.");
			else
				logging.Warning("Disarm.ini not found. Using Default Values.");


			log = blist.GetValue<bool>("Settings", "Logging", false);
			if (log)
				logging.Info("Logging is Enabled.");
			else

				logging.Info("Logging is Disabled. Won't Log anything!");



            string[] blacklistedModels = ReadModels(blist.GetValue<string>("Blacklisted Peds", "Ped Models", ""));
            BlacklistedPeds = blacklistedModels.Select(model => new Model(model)).ToArray();

            if (log)
				logging.Info($"Reading Values for Blacklisted Peds. They are: {blist.GetValue<String>("Blacklisted Peds", "Ped Models", "")}");
			
			hasloaded = true;
		}

		foreach (Ped ped in nearbyPeds)
		{
			//L_Hand 18905, R_Hand 57005, R_Forearm 28252, L_Forearm 61163, R_UpperArm 40269, L_UpperArm 45509

			//Peds will drop weapon if hurt at those Bones.
			//Removed the Left Arm Bones as its just for Holding Purpose.
			//As real task is done by Right Arm/Hand the Grip Holding.

			//We will even ignore this check for peds which are heavy - Juggernauts useful for RDE Mod.

			if (ped.GetLastDamageBone() == 18905 ||
			ped.GetLastDamageBone() == 57005 ||
			ped.GetLastDamageBone() == 45509 ||  
ped.GetLastDamageBone() == 40269 ||
ped.GetLastDamageBone() == 28252 ||
ped.GetLastDamageBone() == 61163 || 
            ped.HasBeenDamagedByWeapon(WeaponHash.StunGun))
			{
    if (!BlacklistedPeds.Contains(ped.Model))
    {
        ped.PlayAmbientSpeech("GENERIC_CURSE_MED", false);
        ped.Weapons.Drop();
					ped.ClearLastDamageBone();
					ped.ClearLastWeaponDamage();
				}
			}
		}
	}
}
*/



namespace Disarm
{
    public class Main : Script
    {
        private bool hasLoaded = false;
        private Logger logging;

        public Main()
        {
            Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            try
            {
                // Load settings if not already loaded
                if (!hasLoaded)
                {
                    LoadSettings();
                    hasLoaded = true;
                }

                // Get nearby peds
                Ped[] nearbyPeds = World.GetNearbyPeds(Game.Player.Character.Position, 500f);

                foreach (Ped ped in nearbyPeds)
                {
                    DisarmPed(ped); // Disarm each nearby ped
                }
            }
            catch (Exception ex)
            {
                logging.Error($"An error occurred: {ex.Message}");
            }
        }

        private void LoadSettings()
        {
            logging = new Logger("./scripts/Disarm.log");

            ScriptSettings settings = ScriptSettings.Load("./scripts/Disarm.ini");

            if (File.Exists("./scripts/Disarm.ini"))
            {
                logging.Info("Disarm.ini Found. Setting Up Values.");
            }
            else
            {
                logging.Warning("Disarm.ini not found. Using Default Values.");
            }

            log = settings.GetValue<bool>("Settings", "Logging", false);

            if (log)
            {
                logging.Info("Logging is Enabled.");
            }
            else
            {
                logging.Info("Logging is Disabled. Won't Log anything!");
            }

            string[] blacklistedModels = ReadModels(settings.GetValue<string>("Blacklisted Peds", "Ped Models", ""));
            BlacklistedPeds = blacklistedModels.Select(model => new Model(model)).ToArray();

            logging.Info($"Reading Values for Blacklisted Peds. They are: {string.Join(", ", blacklistedModels)}");
        }

        private string[] ReadModels(string input)
        {
            string[] models = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < models.Length; i++)
            {
                models[i] = models[i].Trim();
            }
            return models;
        }

        private void DisarmPed(Ped ped)
        {
            if (!BlacklistedPeds.Contains(ped.Model))
            {
                if (IsHitOnArm(ped.GetLastDamageBone()))
                {
                    ped.PlayAmbientSpeech("GENERIC_CURSE_MED", false);
                    ped.Weapons.Drop();
                    ped.ClearLastDamageBone();
                    ped.ClearLastWeaponDamage();
                }
            }
        }

        private bool IsHitOnArm(int boneId)
        {
            int[] armBones = { 18905, 57005, 28252, 61163, 40269, 45509 };
            return armBones.Contains(boneId);
        }

        public static Model[] BlacklistedPeds = { };
        public static bool log = false;
    }
}
