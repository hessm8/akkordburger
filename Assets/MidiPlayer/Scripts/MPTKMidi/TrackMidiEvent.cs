namespace MidiToolkit {
    //! @cond NODOC

    /// <summary>
    /// Midi event list (NAUdio format)
    /// </summary>
    public class TrackMidiEvent
    {
        public int IndexTrack;
        public long AbsoluteQuantize;
        public MPTK.NAudio.Midi.MidiEvent Event;
    }

    //! @endcond

}
