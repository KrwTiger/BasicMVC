/***********************************
TODO:
====
-   Effects on hover over cheezy Toasts
-   Re-arrange the examples.
-   Make standardized id/class names.(Use hyphen(-) instead of underscore(_))
-   Prepare proper code block examples.
-   Clean up global variables, wherever possible.
-   Testing on various platforms.
-   DONE    Error logging on console.
-   DONE    Add option to display toast at the top and bottom.
-   DONE    Manage multiple toast execution.
-   DONE    Resolve overlapping style problems. (Click Timed Toast, Styled toast and Timed Toast again.)
-   DONE    Clear all the TODOs.
-   DONE    Style the home page.
-   DONE    Make Cheezy Toast.

IDEAS:
=====
-   More use cases to be thought of.(error handling, Welcome Messages, Touring helpers, Validation responses, status updates)
-   More templates can be added on the top(like procedures which are hardcoded to run the same way).
-   Toast selectors can also made so that the various toasts are handled by the script itself.
-   (Think about)/(Test the) responsiveness of the Toast.
-   Can textboxes and other stuff be added too? That would mean, that it's the real replacement for alert boxes.
-   Should the toasts be allowed to move as well? That would make it like stick-its(Task list) kind of thing.
***********************************/

/**
    This is the queue data structure.
**/
var queue = [];

/**
    Default delay value.
**/
var default_delay = 3000;

var default_toast_type = "default";
var acceptable_toast_type_values = new Array("cheezy", "default");

/**
    Default id handles.
**/
var default_id = "kamehameha_toast";
var default_id_handle = "#" + default_id;
var default_wrapper_id = "kamehameha_wrapper";
var default_wrapper_id_handle = "#" + default_wrapper_id;
var default_cross_mark_code = "<div id=\"kamehameha_cross\">&#10060;</div>";

/**
    This dict is used to set the default style of the dictionary.
**/
var default_toast_style_dict =
{
    "font-size": "15px",
    "color": "#FFF",
    "margin-left": "auto",
    "padding": "10px 10px",
    "margin-right": "auto",
    "max-width": "40%",
    "font-family": "'Helvetica Neue', Helvetica, Arial, sans-serif",
    "display": "none",
    "background-color": "#000",
    "background-color": "rgba(0,0,0,0.8)",
    "text-align": "center"
};

var default_wrapper_style_dict = 
{
    "position" : "fixed",
    "width" : "100%",
    "bottom" : "2%",
    "text-align" : "center"
};

var default_cheezy_mouseenter_style_dict =
{
    "border"  :   "2px dashed rgb(0, 0, 0)",
    "border"  :   "2px dashed rgba(0, 0, 0, 0.4)",
};

var default_cheezy_mouseexit_style_dict =
{
    "border-width"  :   "0px"
};

/**
    ()  ->  None
    Initialising the elements that should be in the Toast.
    Basically appends a div in the body with the predined style.
**/
function initialise_toast(){
    element = "<div id=\"" + default_wrapper_id + "\"><div id=\"" + default_id + "\"></div></div>";
    $("body").append(element);
    $("<style type='text/css'> .mouseenter-style{} </style>").appendTo("head");
}

/**
    (string, *{...}) -> None
    Takes the string that is to be displayed and optional argument.
    The optional argument can contain delay information and styling details.
    eg : {"delay":3000, "padding":"30px"}
    This function shows a message for a short period of time bottom of the browser.
    Can be used for debugging, or in general giving some failure messages.
**/ 
function toast(toast_str, opt_args){
    if(typeof toast_str === "undefined"){
        log_e("Toast display string needs to be passed as parameter.");
        return;
    }
    var id_handle = default_id_handle;
    var wrapper_id_handle = default_wrapper_id_handle;

    var delay = default_delay;
    var toast_type = default_toast_type;

    //Shallow copy is required.
    var style_dict = JSON.parse(JSON.stringify(default_toast_style_dict));
    var wrapper_style_dict = JSON.parse(JSON.stringify(default_wrapper_style_dict));


    if(typeof opt_args  !== "undefined"){
        var_dict = populate_style_values(opt_args, wrapper_style_dict, style_dict);
        delay = var_dict['delay'];
        toast_type = var_dict['toast_type'];
        log(toast_type);
    }
    queue_params = {
        "id_handle" :   id_handle,
        "wrapper_id_handle" : wrapper_id_handle,
        "toast_str" :   toast_str,
        "style_dict":   style_dict,
        "wrapper_style_dict" : wrapper_style_dict,
        "delay"     :   delay,
        "toast_type":   toast_type,
        "speed"     :   "slow"
    };

    //add params to queue.
    add_to_queue(queue_params);
}

/**
    ({...}) ->  None
    This adds the queue_params to the queue, and proceeds to queue processing.
**/
function add_to_queue(queue_params){
    if(typeof queue_params === "undefined"){
        log_e("Queue needs parameters to be logged.");
        return;
    }
    queue.unshift(queue_params);
    log(JSON.stringify(queue));
    if(queue.length == 1){
        queue_processing();
    }
}

/**
    (*{...})    ->  None
    The actual display logic is here.
    Takes care of popping the queue, and displaying the content.
**/
function queue_processing(first_time){
    if(typeof first_time === "undefined"){
        first_time = true;
    }
    if(queue.length == 0){
        log("Nothing to display");
        return;
    }
    if(queue.length != 1){
        queue.pop();
    }else{
        if(first_time){
            first_time = false;
        }else{
            first_time = true;
            queue.pop();
            return;
        }
    }
    display_dict = queue[queue.length - 1];
    //log("printing -- " + JSON.stringify(display_dict));

    $(display_dict["wrapper_id_handle"]).removeAttr("style").css(display_dict['wrapper_style_dict']);

    if(display_dict['toast_type'] == "cheezy"){
        display_dict['style_dict']['cursor'] = "pointer";
        $(display_dict["id_handle"]).
            removeAttr("style").
            text(display_dict["toast_str"]).
            css(display_dict["style_dict"]).
            fadeIn(display_dict["speed"]).
            bind(
                {
                    mouseenter:function(){
                        $(this).css(default_cheezy_mouseenter_style_dict);
                    },
                    mouseleave:function(){
                        $(this).css(default_cheezy_mouseexit_style_dict);
                    },
                    click:function(){
                        $(this).fadeOut(display_dict["speed"], function(){queue_processing(first_time);});
                    }
                }
            )
        ;
    }else if(display_dict['toast_type'] == "default"){
        $(display_dict["id_handle"]).
            removeAttr("style").
            text(display_dict["toast_str"]).
            css(display_dict["style_dict"]).
            fadeIn(display_dict["speed"]).
            delay(display_dict["delay"]).
            fadeOut(display_dict["speed"], function(){
                    queue_processing(first_time);
                }
            )
        ;
    }else{
        log_e("Invalid Toast-type Values");
    }
}

/**
    ({...}) ->  None
    Sets the default values as specified in the arguments.
**/
function set_default(args){
    if(typeof args === "undefined"){
        return;
    }
    var_dict = populate_style_values(args, default_wrapper_style_dict, default_toast_style_dict);
    default_delay = var_dict['delay'];
    default_toast_type = var_dict['toast_type'];
}

function populate_style_values(args,wrapper_style_dict, toast_style_dict){
    var delay_var = default_delay;
    var toast_type_var = default_toast_type;
    for(key in args){
        switch(key){
            case "delay"    :   
                                if(isNaN(args[key]) === false){
                                    delay_var = args[key];    
                                }
                                break;
            case "toast-pos" :
                                delete wrapper_style_dict['top'];
                                delete wrapper_style_dict['bottom'];
                                switch(args[key]){
                                    case "top"   :
                                                wrapper_style_dict['top'] = "2%";
                                                break;
                                    case "bottom":
                                                wrapper_style_dict['bottom'] = "2%";
                                                break;
                                };
                                break;
            case "toast-type"  :
                                if(acceptable_toast_type_values.indexOf(args[key]) > -1){
                                    toast_type_var = args[key];
                                }
                                break;
            default         :
                                toast_style_dict[key] = args[key];
                                break;
        };
    }
    return  {
                "delay"         :  delay_var,
                "toast_type"    :  toast_type_var
            };
}


/**
    (string)    ->  None
    Logging function
**/
function log(str){
    if(console){
        console.log(str);
    }
}

/**
    (string)    ->  None
    Error logging function
**/
function log_e(str){
    if(console){
        console.error(str);
    }
}

/**
    Initialising the Toast.
**/
$(document).ready(initialise_toast);