﻿using Nikse.SubtitleEdit.Core;
using System;

namespace Nikse.SubtitleEdit.Logic
{
    public class Paragraph
    {
        public int Number { get; set; }

        public string Text { get; set; }

        public TimeCode StartTime { get; set; }

        public TimeCode EndTime { get; set; }

        public TimeCode Duration
        {
            get
            {
                var timeCode = new TimeCode(EndTime.TotalMilliseconds);
                timeCode.AddTime(-StartTime.TotalMilliseconds);
                return timeCode;
            }
        }

        public int StartFrame { get; set; }

        public int EndFrame { get; set; }

        public bool Forced { get; set; }

        public string Extra { get; set; }

        public bool IsComment { get; set; }

        public string Actor { get; set; }

        public string Effect { get; set; }

        public int Layer { get; set; }

        public string ID { get; set; }

        public string Language { get; set; }

        public string Style { get; set; }

        public bool NewSection { get; set; }

        public Paragraph()
        {
            StartTime = TimeCode.FromSeconds(0);
            EndTime = TimeCode.FromSeconds(0);
            Text = string.Empty;
            ID = Guid.NewGuid().ToString();
        }

        public Paragraph(TimeCode startTime, TimeCode endTime, string text)
        {
            StartTime = startTime;
            EndTime = endTime;
            Text = text;
            ID = Guid.NewGuid().ToString();
        }

        public Paragraph(Paragraph paragraph)
        {
            Number = paragraph.Number;
            Text = paragraph.Text;
            StartTime = new TimeCode(paragraph.StartTime.TotalMilliseconds);
            EndTime = new TimeCode(paragraph.EndTime.TotalMilliseconds);
            StartFrame = paragraph.StartFrame;
            EndFrame = paragraph.EndFrame;
            Forced = paragraph.Forced;
            Extra = paragraph.Extra;
            IsComment = paragraph.IsComment;
            Actor = paragraph.Actor;
            Effect = paragraph.Effect;
            Layer = paragraph.Layer;
            ID = paragraph.ID;
            Language = paragraph.Language;
            Style = paragraph.Style;
            NewSection = paragraph.NewSection;
        }

        public Paragraph(int startFrame, int endFrame, string text)
        {
            StartTime = new TimeCode(0, 0, 0, 0);
            EndTime = new TimeCode(0, 0, 0, 0);
            StartFrame = startFrame;
            EndFrame = endFrame;
            Text = text;
            ID = Guid.NewGuid().ToString();
        }

        public Paragraph(string text, double startTotalMilliseconds, double endTotalMilliseconds)
        {
            StartTime = new TimeCode(startTotalMilliseconds);
            EndTime = new TimeCode(endTotalMilliseconds);
            Text = text;
            ID = Guid.NewGuid().ToString();
        }

        internal void Adjust(double factor, double adjust)
        {
            if (StartTime.IsMaxTime)
                return;

            double seconds = StartTime.TimeSpan.TotalSeconds * factor + adjust;
            StartTime.TimeSpan = TimeSpan.FromSeconds(seconds);

            seconds = EndTime.TimeSpan.TotalSeconds * factor + adjust;
            EndTime.TimeSpan = TimeSpan.FromSeconds(seconds);
        }

        public void CalculateFrameNumbersFromTimeCodes(double frameRate)
        {
            StartFrame = (int)Math.Round((StartTime.TotalMilliseconds / TimeCode.BaseUnit * frameRate));
            EndFrame = (int)Math.Round((EndTime.TotalMilliseconds / TimeCode.BaseUnit * frameRate));
        }

        public void CalculateTimeCodesFromFrameNumbers(double frameRate)
        {
            StartTime.TotalMilliseconds = StartFrame * (TimeCode.BaseUnit / frameRate);
            EndTime.TotalMilliseconds = EndFrame * (TimeCode.BaseUnit / frameRate);
        }

        public override string ToString()
        {
            return StartTime + " --> " + EndTime + " " + Text;
        }

        public int NumberOfLines
        {
            get
            {
                return Utilities.GetNumberOfLines(Text);
            }
        }

        public double WordsPerMinute
        {
            get
            {
                if (string.IsNullOrEmpty(Text))
                    return 0;
                int wordCount = HtmlUtil.RemoveHtmlTags(Text, true).Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '(', ')', '[', ']', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                return (60.0 / Duration.TotalSeconds) * wordCount;
            }
        }
    }
}