function Select2Languages() {
    return {
        errorLoading: function () {
            return "Sonuç yüklenemedi"
        },
        inputTooLong: function (n) {
            return n.input.length - n.maximum + " karakter daha girmelisiniz"
        },
        inputTooShort: function (n) {
            return "En az " + (n.minimum - n.input.length) + " karakter daha girmelisiniz"
        },
        loadingMore: function () {
            return "Daha fazla…"
        },
        maximumSelected: function (n) {
            return "Sadece " + n.maximum + " seçim yapabilirsiniz"
        },
        noResults: function () {
            return "Sonuç bulunamadı"
        },
        searching: function () {
            return "Aranıyor…"
        },
        removeAllItems: function () {
            return "Tüm öğeleri kaldır"
        }
    }
}


/* Add here all your JS customizations */
function addAntiForgeryToken(data) {
    //if the object is undefined, create a new one.
    if (!data) {
        data = {};
    }
    //add token
    var tokenInput = $('input[name=__RequestVerificationToken]');
    if (tokenInput.length) {
        data.__RequestVerificationToken = tokenInput.val();
    }
    return data;
};
function display_kendoui_grid_error(e) {
    if (e.errors) {
        if ((typeof e.errors) == 'string') {
            //single error
            //display the message
            alert(e.errors);
        } else {
            //array of errors
            //source: http://docs.kendoui.com/getting-started/using-kendo-with/aspnet-mvc/helpers/grid/faq#how-do-i-display-model-state-errors?
            var message = "The following errors have occurred:";
            //create a message containing all errors.
            $.each(e.errors, function (key, value) {
                if (value.errors) {
                    message += "\n";
                    message += value.errors.join("\n");
                }
            });
            //display the message
            alert(message);
        }
        //ignore empty error
    } else if (e.errorThrown) {
        alert('Error happened');
    }
}


function bindBootstrapTabSelectEvent(tabsId, inputId) {
    $('#' + tabsId + ' > ul li a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var tabName = $(e.target).attr("data-tab-name");
        $("#" + inputId).val(tabName);
    });
}