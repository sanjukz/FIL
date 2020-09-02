using System;
using System.Collections.Generic;

public class guid
{
    public string rendered { get; set; }

    public static object NewGuid()
    {
        throw new NotImplementedException();
    }
}

public class Title
{
    public string rendered { get; set; }
}

public class Content
{
    public string rendered { get; set; }
    public bool Protected { get; set; }
}

public class Excerpt
{
    public string rendered { get; set; }
    public bool Protected { get; set; }
}

public class Meta
{
    public string advanced_seo_description { get; set; }
    public string amp_status { get; set; }
    public string spay_email { get; set; }
    public string jetpack_publicize_message { get; set; }
}

public class Self
{
    public string href { get; set; }
}

public class Collection
{
    public string href { get; set; }
}

public class About
{
    public string href { get; set; }
}

public class Author
{
    public bool embeddable { get; set; }
    public string href { get; set; }
}

public class Reply
{
    public bool embeddable { get; set; }
    public string href { get; set; }
}

public class WpTerm
{
    public string taxonomy { get; set; }
    public bool embeddable { get; set; }
    public string href { get; set; }
}

public class Cury
{
    public string name { get; set; }
    public string href { get; set; }
    public bool templated { get; set; }
}

public class Links
{
    public IList<Self> self { get; set; }
    public IList<Collection> collection { get; set; }
    public IList<About> about { get; set; }
    public IList<Author> author { get; set; }
    public IList<Reply> replies { get; set; }
    public IList<Cury> curies { get; set; }
}

public class BlogResponseModel
{
    public int id { get; set; }
    public DateTime date { get; set; }
    public DateTime date_gmt { get; set; }
    public guid guid { get; set; }
    public DateTime modified { get; set; }
    public DateTime modified_gmt { get; set; }
    public string slug { get; set; }
    public string status { get; set; }
    public string type { get; set; }
    public string link { get; set; }
    public Title title { get; set; }
    public Content content { get; set; }
    public Excerpt excerpt { get; set; }
    public int author { get; set; }
    public int featured_media { get; set; }
    public string comment_status { get; set; }
    public string ping_status { get; set; }
    public bool sticky { get; set; }
    public string template { get; set; }
    public string format { get; set; }
    public Meta meta { get; set; }
    public IList<int> categories { get; set; }
    public IList<int> tags { get; set; }
    public string jetpack_featured_media_url { get; set; }
    public IList<object> jetpack_publicize_connections { get; set; }
    public string yoast_head { get; set; }
    public bool jetpack_likes_enabled { get; set; }
    public bool jetpack_sharing_enabled { get; set; }
    public string jetpack_shortlink { get; set; }
    public Links _links { get; set; }
    public bool? IsImageUpload { get; set; }
}