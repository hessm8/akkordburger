using System;
using System.Collections.Generic;

using MidiToolkit;

public struct Instrument {

    public static MidiStreamPlayer player => General.AudioManager.Player;

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

    public readonly static List<Instrument> Presets = new List<Instrument>() {
        new Instrument("Piano", 0, 0),
        new Instrument("Harpsichord", 0, 6),
        new Instrument("Glockenspiel", 1, 9),
        new Instrument("Accordion", 2, 21),
        new Instrument("Acoustic Guitar", 3, 25),
        new Instrument("Rock Guitar", 3, 29),
        new Instrument("Bass Guitar", 4, 36),
        new Instrument("Harp", 5, 46),
        new Instrument("Strings", 6, 48),
        new Instrument("Drums", 9, 0),
        new Instrument("Synth Brass", 7, 62),
        new Instrument("Synth Square", 10, 80),
        new Instrument("Synth Sawtooth", 10, 81)
    };
}