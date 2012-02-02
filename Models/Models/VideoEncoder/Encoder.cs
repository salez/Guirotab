using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;


namespace VideoEncoder
{
    public class Encoder
    {

        public EncodedVideo EncodeVideo(VideoFile input, string encodingCommand, string outputFile, bool getVideoThumbnail)
        {
            EncodedVideo encoded = new EncodedVideo();

            Params = string.Format("-i {0} {1} {2}", input.Path, encodingCommand, outputFile);
            string output = RunProcess(Params);
            encoded.EncodingLog = output;
            encoded.EncodedVideoPath = outputFile;

            if (File.Exists(outputFile))
            {
                encoded.Success = true;

                //get thumbnail?
                if (getVideoThumbnail)
                {
                    string saveThumbnailTo = outputFile + "_thumb.jpg";

                    if (GetVideoThumbnail(input, saveThumbnailTo,"1024x768", 1))
                    {
                        encoded.ThumbnailPath = saveThumbnailTo;
                    }
                }
            }
            else
            {
                encoded.Success = false;
            }


            return encoded;

        }
        public bool GetVideoThumbnail(VideoFile input, string saveThumbnailTo, string tamanho, int secs)
        {
            if (!input.infoGathered)
            {
                GetVideoInfo(input);
            }

            //divide the duration in 3 to get a preview image in the middle of the clip
            //instead of a black image from the beginning.
            //secs = (int)Math.Round(TimeSpan.FromTicks(input.Duration.Ticks / 3).TotalSeconds, 0);
            //if (secs.Equals(0)) secs = 1;

            string Params = string.Format("-i \"{0}\" -s 1024x768 -vcodec mjpeg -ss {2} -vframes 1 -an -f image2 \"{1}\"", input.Path, saveThumbnailTo, secs, tamanho);
            string output = RunProcess(Params);

            if (File.Exists(saveThumbnailTo))
            {
                return true;
            }
            else
            {
                //try running again at frame 1 to get something
                Params = string.Format("-i \"{0}\" -s 1024x768 -vcodec mjpeg -ss {2} -vframes 1 -an -f image2 \"{1}\"", input.Path, saveThumbnailTo, 1, tamanho);
                output = RunProcess(Params);

                if (File.Exists(saveThumbnailTo))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void GetVideoInfo(VideoFile input)
        {
            string Params = string.Format("-i {0}", input.Path);
            string output = RunProcess(Params);
            input.RawInfo = output;
            input.Duration = ExtractDuration(input.RawInfo);
            input.BitRate = ExtractBitrate(input.RawInfo);
            input.RawAudioFormat = ExtractRawAudioFormat(input.RawInfo);
            input.AudioFormat = ExtractAudioFormat(input.RawAudioFormat);
            input.RawVideoFormat = ExtractRawVideoFormat(input.RawInfo);
            input.VideoFormat = ExtractVideoFormat(input.RawVideoFormat);
            input.Width = ExtractVideoWidth(input.RawInfo);
            input.Height = ExtractVideoHeight(input.RawInfo);
            input.infoGathered = true;
        }
        private string RunProcess(string Parameters)
        {
            //create a process info
            ProcessStartInfo oInfo = new ProcessStartInfo(this.FFmpegPath, Parameters);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            //Create the output and streamreader to get the output
            string output = null; StreamReader srOutput = null;

            //try the process
            try
            {
                //run the process
                Process proc = System.Diagnostics.Process.Start(oInfo);

                proc.WaitForExit();

                //get the output
                srOutput = proc.StandardError;

                //now put it in a string
                output = srOutput.ReadToEnd();

                proc.Close();
            }
            catch (Exception)
            {
                output = string.Empty;
            }
            finally
            {
                //now, if we succeded, close out the streamreader
                if (srOutput != null)
                {
                    srOutput.Close();
                    srOutput.Dispose();
                }
            }
            return output;
        }
        public string FFmpegPath { get; set; }
        private string Params { get; set; }
        private TimeSpan ExtractDuration(string rawInfo)
        {
            TimeSpan t = new TimeSpan(0);
            Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);

            if (m.Success)
            {
                string duration = m.Groups[1].Value;
                string[] timepieces = duration.Split(new char[] { ':', '.' });
                if (timepieces.Length == 4)
                {
                    t = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]), Convert.ToInt16(timepieces[3]));
                }
            }

            return t;
        }
        private double ExtractBitrate(string rawInfo)
        {
            Regex re = new Regex("[B|b]itrate:.((\\d|:)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            double kb = 0.0;
            if (m.Success)
            {
                Double.TryParse(m.Groups[1].Value, out kb);
            }
            return kb;
        }
        private string ExtractRawAudioFormat(string rawInfo)
        {
            string a = string.Empty;
            Regex re = new Regex("[A|a]udio:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                a = m.Value;
            }
            return a.Replace("Audio: ", "");
        }
        private string ExtractAudioFormat(string rawAudioFormat)
        {
            string[] parts = rawAudioFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Audio: ", "");
        }
        private string ExtractRawVideoFormat(string rawInfo)
        {
            string v = string.Empty;
            Regex re = new Regex("[V|v]ideo:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                v = m.Value;
            }
            return v.Replace("Video: ", ""); ;
        }
        private string ExtractVideoFormat(string rawVideoFormat)
        {
            string[] parts = rawVideoFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Video: ", "");
        }
        private int ExtractVideoWidth(string rawInfo)
        {
            int width = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out width);
            }
            return width;
        }
        private int ExtractVideoHeight(string rawInfo)
        {
            int height = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[2].Value, out height);
            }
            return height;
        }
    }
}