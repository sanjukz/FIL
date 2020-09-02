export interface BlogPost {
    id:                            number;
    date:                          Date;
    date_gmt:                      Date;
    guid:                          GUID;
    modified:                      Date;
    modified_gmt:                  Date;
    slug:                          string;
    status:                        string;
    type:                          string;
    link:                          string;
    title:                         GUID;
    content:                       Content;
    excerpt:                       Content;
    author:                        number;
    featured_media:                number;
    comment_status:                string;
    ping_status:                   string;
    sticky:                        boolean;
    template:                      string;
    format:                        string;
    meta:                          Meta;
    categories:                    number[];
    tags:                          any[];
    jetpack_featured_media_url:    string;
    jetpack_publicize_connections: any[];
    jetpack_likes_enabled:         boolean;
    jetpack_sharing_enabled:       boolean;
    jetpack_shortlink:             string;
    _links:                        Links;
}

interface Links {
    self:                  About[];
    collection:            About[];
    about:                 About[];
    author:                Author[];
    replies:               Author[];
    "version-history":     VersionHistory[];
    "predecessor-version": PredecessorVersion[];
    "wp:featuredmedia":    Author[];
    "wp:attachment":       About[];
    "wp:term":             WpTerm[];
    curies:                Cury[];
}

interface About {
    href: string;
}

interface Author {
    embeddable: boolean;
    href:       string;
}

interface Cury {
    name:      string;
    href:      string;
    templated: boolean;
}

interface PredecessorVersion {
    id:   number;
    href: string;
}

interface VersionHistory {
    count: number;
    href:  string;
}

interface WpTerm {
    taxonomy:   string;
    embeddable: boolean;
    href:       string;
}

interface Content {
    rendered:  string;
    protected: boolean;
}

interface GUID {
    rendered: string;
}

interface Meta {
    amp_status:                string;
    spay_email:                string;
    jetpack_publicize_message: string;
}
