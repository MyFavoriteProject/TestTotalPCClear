using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCleaner.Model
{
    public static class Constants
    {
        public static class FileTypeGlyph
        {
            static Random random = new Random();

            public const string SystemCacheGlyph = "\ue770";
            public const string ApplicationCacheGlyph = "\ue74C";
            public const string MailCacheGlyph = "\ue715";
            public const string OfficeCacheGlyph = "\ue56E";
            public const string BrowserCacheGlyph = "\ue774";
            public const string FolderGlyph = "\ue838";
            public const string JustGlyph = "\ue790";

            public static string GetGlyph()
            {
                string result = null;
                int index = random.Next(0, 6);

                switch (index)
                {
                    case 0:
                        result = SystemCacheGlyph;
                        break;
                    case 1:
                        result = ApplicationCacheGlyph;
                        break;
                    case 2:
                        result = MailCacheGlyph;
                        break;
                    case 3:
                        result = OfficeCacheGlyph;
                        break;
                    case 4:
                        result = BrowserCacheGlyph;
                        break;
                    case 5:
                        result = FolderGlyph;
                        break;
                    case 6:
                        result = JustGlyph;
                        break;
                }

                return result;
            }
        }
    }
}
