using System.Runtime.Serialization;

namespace LaserAPI.Models.Helper
{
    public struct LaserCommando
    {
        /// <summary>
        /// The time the laser should project the pattern, if set to -1 the laser will keep projecting until a commando with the stop property set to true is send
        /// </summary>
        [DataMember(Name = "dms")]
        public long DurationInMilliseconds { get; set; }

        /// <summary>
        /// The messages the laser should send
        /// </summary>
        [DataMember(Name = "m")]
        public LaserMessage[] Messages { get; set; }

        /// <summary>
        /// If set to true the laser will stop playing the pattern
        /// </summary>
        [DataMember(Name = "sp")]
        public bool Stop { get; set; }

        /// <summary>
        /// A commando that can be send to the laser
        /// </summary>
        /// <param name="durationInMilliseconds">The time the laser should project the pattern, if set to -1 the laser will keep projecting until a commando with the stop property set to true is send</param>
        /// <param name="messages">The messages the laser should send</param>
        /// <param name="stop">If set to true the laser will stop playing the pattern</param>
        public LaserCommando(int durationInMilliseconds, LaserMessage[] messages, bool stop = false)
        {
            DurationInMilliseconds = durationInMilliseconds;
            Messages = messages;
            Stop = stop;
        }
    }
}
