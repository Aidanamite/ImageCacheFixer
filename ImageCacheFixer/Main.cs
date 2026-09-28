using BepInEx;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ImageCacheFixer
{
    [BepInPlugin("com.aidanamite.ImageCacheFixer", "Image Cache Fixer", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        public void Awake()
        {
            new Harmony("com.aidanamite.ImageCacheFixer").PatchAll();
            Logger.LogInfo("Loaded");
        }

        public static void EnsureSize<T>(ref T[] array, int index)
        {
            if (array.Length <= index)
                Array.Resize(ref array, index + 1);
        }
    }

    [HarmonyPatch]
    static class Patch_ImageData
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(ImageData), "Load", new[] { typeof(string), typeof(int), typeof(GameObject), typeof(string) });
            yield return AccessTools.Method(typeof(ImageData), "GetImage");
            yield return AccessTools.Method(typeof(ImageData), "Save");
            yield return AccessTools.Method(typeof(ImageData), "SetImage");
        }
        static void Prefix(string iType, int slotIdx)
        {
                 if (iType == "Drawing")      Main.EnsureSize(ref ImageData.mArtFinal,  slotIdx);
            else if (iType == "DrawingColor") Main.EnsureSize(ref ImageData.mArtColor,  slotIdx);
            else if (iType == "Photo")        Main.EnsureSize(ref ImageData.mPhoto,     slotIdx);
            else if (iType == "Billboard")    Main.EnsureSize(ref ImageData.mBillboard, slotIdx);
            else if (iType == "EggColor")     Main.EnsureSize(ref ImageData.mEggColor,  slotIdx);
            else if (iType == "SWTrack")      Main.EnsureSize(ref ImageData.mSWTracks,  slotIdx);
            else if (iType == "Avatar")       Main.EnsureSize(ref ImageData.mAvatars,   slotIdx);
        }
    }
}