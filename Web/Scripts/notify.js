var notify = function(message, type, layout) {
    type = type || "success";
    layout = layout || "topRight";

    noty({
        "text": message,
        "layout": layout,
        "type": type,
        "textAlign": "center",
        "easing": "swing",
        "animateOpen": { "height": "toggle", "opacity": 0.7 },
        "animateClose": { "height": "toggle" },
        "speed": "500",
        "timeout": "5000",
        "closable": true,
        "closeOnSelfClick": true
    });
};