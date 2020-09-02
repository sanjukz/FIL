// use with single variables
export const pushData = (variable, data) => {
    var win = (window || {}) as any;
    var dataLayer = win.dataLayer || [];
    dataLayer.push({variable, data});
}

// For use with multiple variables
export const pushObject = (data) => {
    var win = (window || {}) as any;
    var dataLayer = win.dataLayer || [];
    dataLayer.push(data);
}
