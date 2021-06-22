namespace TweetStatsViewer.Models
{
    public class Emoji
    {
        public string Name { get; set; }

        public string Unified { get; set; }

        public string Non_qualified { get; set; }

        public string Docomo { get; set; }

        public string Au { get; set; }

        public string Softbank { get; set; }

        public string Google { get; set; }

        public string Image { get; set; }

        public short Sheet_x { get; set; }

        public short Sheet_y { get; set; }

        public string Short_name { get; set; }

        public string[] Short_names { get; set; }

        public string Text { get; set; }

        public string[] Texts { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public int Sort_order { get; set; }

        public string Added_in { get; set; }

        public bool Has_img_apple { get; set; }

        public bool Has_img_google { get; set; }

        public bool Has_img_twitter { get; set; }

        public bool Has_img_facebook { get; set; }
    }
}
