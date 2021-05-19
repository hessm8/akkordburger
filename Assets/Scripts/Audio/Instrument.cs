using System.Collections.Generic;

using MidiToolkit;

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

    public readonly static List<Instrument> Presets = new List<Instrument>() {
        new Instrument("Piano", 0, 0),
        new Instrument("Harpsichord", 0, 6),
        new Instrument("Glockenspiel", 1, 9),
        new Instrument("Accordion", 2, 21),
        new Instrument("Acoustic Guitar", 3, 24),
        new Instrument("Rock Guitar", 3, 29)
    };
}