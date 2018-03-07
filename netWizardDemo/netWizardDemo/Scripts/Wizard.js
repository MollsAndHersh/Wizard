(function ($) {
    $.fn.Wizard = function (options) {
        $(this).each(function () {
            var form = $(this);
            //Initialize the form, build the options oject, and set data values
            Init(form,options);
            var opts = form.data("options");
            var nav = $(".wizard-form-nav", form);
            var formPages = $(".wizard-form-pages",form);
            var fieldsets = $("fieldset", formPages);
            var btn = $("<input type='button' value='' class='btn wizard-nav-button' />");
            var div = $("<div class='btn-group'></div");
            var ajaxCount = 0;
            //Load the ajax links for each fieldset, build the nav buttons, and prase the new ajax elements for validation
            fieldsets.each(function () {
                var fieldset = $(this);
                fieldset.width(opts.size);
                var btnClone = btn.clone()
                                  .prop("value", fieldset.attr("name"))
                                  .prop("name", fieldset.attr("name"))
                                  .prop("disabled", true)
                                  .addClass(opts.buttons.classes.Nav);

                var divClone = div.clone().addClass("btn-group-sm").append(btnClone);
                nav.append(divClone);
                btnClone.on("click", function () {
                    var index = $(this).parent().index() + 1;
                    $(".active",nav).toggleClass("active");
                    $(this).toggleClass("active");
                    form.data("wizard-current-index", index);
                    JumpToPage(form);
                });
                
                var url = fieldset.data("url");
                fieldset.attr("data-wizard-page-num", fieldsets.index(fieldset) + 1);
                if (url != null) {
                    $.ajax({
                        type: "POST",
                        url: url,
                        success: function (response) {
                            fieldset.html(response);
                            //ajaxCount++;
                            //if (ajaxCount === fieldsets.length) {
                                $(".wizard-addable", fieldset).Addable(0, form);
                                ResetFormValidation(form);
                               // ajaxCount = 0;
                            //}
                        }
                    })
                }
            });

            //Set up the control buttons
            $("div:nth-child(1) input", nav).toggleClass("active").prop("disabled",false);
            var controls = $(".wizard-form-controls", form);

            var prev = btn.clone().prop("value", "Previous").addClass(opts.buttons.classes.Previous).prop("name","Previous").prop("disabled", true);
            var divClone = div.clone().append(prev);
            controls.append(divClone);

            var submit = btn.clone().prop("type", "submit").prop("value", "Submit").addClass(opts.buttons.classes.Submit);
            divClone = div.clone().append(submit);
            controls.append(divClone);

            var next = btn.clone().prop("value", "Next").addClass(opts.buttons.classes.Next).prop("name", "Next");
            divClone = div.clone().append(next);
            controls.append(divClone);

            next.on("click", function () {
                if (opts.loadOnNext) {
                    OnNext(form);
                }
                ChangePage(form, 1); 
                
            });
            prev.on("click", function () {
                ChangePage(form, -1);
            });

            //when submitted, the nav icons are changed to represent invalid fields
            form.on("submit", function (e) {
                HandleValidation(form);
                if ($(this).valid()) {
                    if ($.isFunction(form.data("options").onSubmit)) {

                        opts.onSubmit.call(this, e,form);
                    }
                }
            })

        });
        return this;
    };
    //This can be used on its own.  Hides and disables a container element until the icon is pressed
    $.fn.Addable = function (count,form) {
        var icon = $("<div><i class='glyphicon glyphicon-plus' style='margin-right:10px'></i></div")
        $(this).each(function () {
            var iconClone = icon.clone().append($(this).data("addable-title"));
            var elems = $(this).hide();
            var elemsClone = elems.clone()
            var modelName = $(this).data("addable-model") ;
            $("label", elems).each(function () {
                var currentFor = $(this).attr("for");
                $(this).attr("for", modelName + "_" + count + "_."+currentFor);
            });
            $("input", elems).each(function () {
                var currentId = $(this).attr("id");
                var currentName = $(this).attr("name");
                $(this).attr("name", modelName + "[" + count + "]." + currentName);
                $(this).attr("id", modelName + "_" + count + "_." + currentId);
                $(this).attr("disabled", true);
            });
            $("select", elems).each(function () {
                var currentId = $(this).attr("id");
                var currentName = $(this).attr("name");
                $(this).attr("name", modelName + "[" + count + "]." + currentName);
                $(this).attr("id", modelName + "_" + count + "_." + currentId);
                $(this).attr("disabled", true);
            });
            $(".field-validation-valid", elems).each(function () {
                var currentFor = $(this).data("valmsg-for");
                $(this).attr("data-valmsg-for", modelName + "[" + count + "]." + currentFor);
            });
            elems.before(iconClone);
            var neededAfter = true;
            iconClone.on("click", function () {
                if (neededAfter) {
                    elems.after(elemsClone);
                    elemsClone.Addable(count + 1, form);
                    neededAfter = false;
                }
                iconClone.attr("disabled", true);
                elems.show();
                $(":disabled", elems).attr("disabled", false);
                //This commented code allows for removing an added element but it does not currently work with the model binder
                //When elements are not visible, they are not passed in the view model.  When this occurs, the indexing on each
                //input/select field is off.  This will eventually be fixed in later releases
                //elems.toggle();
                //if (elems.is(":visible")) {
                //    $(":disabled", elems).attr("disabled", false);
                //} else {
                //    $("input", elems).attr("disabled", true);
                //    $("select", elems).attr("disabled", true);
                //}
                //$("i", this).toggleClass("glyphicon-plus").toggleClass("glyphicon-minus");
            })
        });
        ResetFormValidation(form);
    }
    $.fn.Wizard.defaults = {
        size: 600,
        buttons: {
            classes: {
                Previous: "btn-info",
                Submit: "btn-success",
                Next: "btn-primary",
                Nav: "btn-default",
                ValidationWarning:"btn-warning"
            }
        },
        onSubmit: function () {
            alert("I was submitted");
        },
        loadOnNext: false
    }
    function Init(form, options) {

        var opts = BuildOptions(options);

        var formPages = $(".wizard-form-pages", form);
        var fieldsets = $("fieldset", formPages);
        var widthOffset = fieldsets.css("margin-left").replace("px", "") * 1 +
            fieldsets.css("border-left-width").replace("px", "") * 1 +
            fieldsets.css("padding-left").replace("px", "") * 1;

        form.data("options", opts)
            .data("wizard-current-index", 1)
            .data("wizard-total-pages", fieldsets.length)
            .data("wizard-size-offset", widthOffset)
            .width(opts.size + widthOffset * 2);

        formPages.width((opts.size + widthOffset * 2) * fieldsets.length);
        fieldsets.first().toggleClass("active");
    }
    function BuildOptions(options) {
        var opts = $.fn.Wizard.defaults;
        if (options) {
            opts = $.extend({}, $.fn.Wizard.defaults, options);
            if (options.buttons) {
                opts.buttons = $.extend({}, $.fn.Wizard.defaults.buttons, options.buttons)
                opts.buttons.classes = $.extend({}, $.fn.Wizard.defaults.buttons.classes, options.buttons.classes);
            }
        }
        return opts;
    }
    function ResetFormValidation(form) {
        form.removeData("validator");
        form.removeData("unobtrusiveValidation");
        $.validator.setDefaults({
            
            focusInvalid: false
        });
        $.validator.unobtrusive.parse(form);
    }
    function HandleNextPrevStatus(form) {
        var controls = $(".wizard-form-controls", form);
        var next = $("input[name='Next']", controls);
        var prev = $("input[name='Previous']", controls);
        var currentPageNum = form.data("wizard-current-index");
        if (currentPageNum === form.data("wizard-total-pages")) {
            next.prop("disabled", true);
            prev.prop("disabled", false);
        } else if (currentPageNum === 1) {
            next.prop("disabled", false);
            prev.prop("disabled", true);
        } else {
            next.prop("disabled", false);
            prev.prop("disabled", false);
        }
    }
    function SlideForms(form) {
        var newX = (form.data("wizard-current-index")-1) * (form.data("options").size + form.data("wizard-size-offset") * 2);
        $(".wizard-form-pages", form).css("transform", "translateX(" + newX * -1 + "px)");
    }
    function JumpToPage(form) {
        $("fieldset.active", form).toggleClass("active");
        $("fieldset:nth-child(" + form.data("wizard-current-index") + ")", form).toggleClass("active");
        SlideForms(form);
        HandleNextPrevStatus(form);
    }
    function ChangePage(form,direction)
    {
        //Turn off active fieldset
        $("fieldset.active", form).toggleClass("active");

        //Calculate new index left(-1) or right(1)
        var currentIndex = form.data("wizard-current-index");
        var newIndex;
        if (direction === 1) {
            newIndex = (currentIndex >= form.data("wizard-total-pages")) ? 1 : currentIndex + 1;
        } else if (direction === -1) {
            newIndex = (currentIndex === 1) ? $("fieldset", form).length : currentIndex - 1;
        }
        form.data("wizard-current-index", newIndex);

        //Make the new fieldset active
        $("fieldset:nth-child(" + newIndex + ")", form).toggleClass("active");

        //Handle the buttons
        var nav = $(".wizard-form-nav", form);
        $(".active", nav).toggleClass("active");
        $("div:nth-child("+newIndex+") input", nav).toggleClass("active").prop("disabled",false);
        SlideForms(form);
        HandleNextPrevStatus(form);
    }
    function HandleValidation(form)
    {
        var fieldsets = $("fieldset", form);
        ClearAllNavButtonWarnings(form)
        fieldsets.each(function () {
            var inputs = $("input", this);
            var index = $(this).index() + 1;
            inputs.each(function () {
                if (!$(this).valid()) {
                    ToggleNavButtonWarning(form, index);
                    return false;
                }
            })
        })
    }
    function ClearAllNavButtonWarnings(form) {
        var nav = $(".wizard-form-nav", form);
        $("input", nav).removeClass(form.data("options").buttons.classes.ValidationWarning);
    }
    function ToggleNavButtonWarning(form, index) {
        var nav = $(".wizard-form-nav", form);
        var button = $("div:nth-child(" + index + ") input", nav);
        button.toggleClass(form.data("options").buttons.classes.ValidationWarning);
    }

    //data-wizard-on-next='{"controller":"Home","action":"TabPage"}'
    //{ @class = "form-control route-value", data_route_id = "name" }
    function OnNext(form) {
        var currentIndex = form.data("wizard-current-index");
        var newIndex = (currentIndex >= form.data("wizard-total-pages")) ? 1 : currentIndex + 1;
        var fieldset = $("fieldset.active", form);
        var nextFieldset = $("fieldset:nth-child(" + newIndex + ")", form);
        if (fieldset.data("wizard-on-next") != null) {
            var fieldsetUrl = fieldset.data("wizard-on-next").controller + "\\" +
                fieldset.data("wizard-on-next").action + "?";
            var routeValues = $(".route-value", fieldset);
            routeValues.each(function (index) {

                if (!$(this).prop("disabled")) {
                    fieldsetUrl += $(this).data("route-id") + "=" + $(this).val();
                    if (index != routeValues.length - 1 &&
                        !$(routeValues[index + 1]).prop("disabled")) {
                        fieldsetUrl += "&";
                    }
                }
            });
            alert(fieldsetUrl);
            $.ajax({
                type: "POST",
                url: fieldsetUrl,
                success: function (response) {
                    nextFieldset.html(response);

                    $(".wizard-addable", nextFieldset).Addable(0, form);
                    ResetFormValidation(form);
                }
            })
        }
    }
}(jQuery));
