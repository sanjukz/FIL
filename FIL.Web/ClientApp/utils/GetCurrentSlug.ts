export const getCurrentSlug = () => {
    var slug = "all-top-29";
    if (localStorage.getItem('currentSlug') != null) {
        slug = localStorage.getItem('currentSlug');
    }
    return slug;
}