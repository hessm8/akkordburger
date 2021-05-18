using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;


using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

using MidiToolkit;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Instrument {

    public static MidiStreamPlayer player => Akkordburger.AudioManager.Player;

    public Instrument(string name = "Piano", int channel = 0, int preset = 0) {
        Name = name;
        Channel = channel;
        Preset = preset;
    }

    public string Name { get; }
    public int Channel { get; }
    public int Preset { get; }

    public void Use() {
        player.ChangePreset(Channel, Preset);
    }
}