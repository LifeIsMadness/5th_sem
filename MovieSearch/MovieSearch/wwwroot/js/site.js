// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function sortByGenre(val) {

    $.ajax({
        type: "GET",
        url: "/Movies/IndexSort?genreId=" + val,
        success: function (movies) {
            $("#view-all").html(movies)
        }
    });
}

function searchMovie() {
    search_value = $("#search_inp").val();
    $.ajax({
        type: "GET",
        url: "/Movies/IndexSearch?searchValue=" + search_value,
        success: function (movies) {
            $("#view-all").html(movies);
        }
    });
}

function setRatingColor() {
    $el = $("span.rating-details");
    $el.each(function (i, item) {
        var val = $(this).html().replace(',', '.');
        if (val >= 0 && val < 5) {
            $(this).removeClass("span-red span-green span-yellow").addClass("span-red");

        }
        else if (val >= 5 && val < 8) {
            $(this).removeClass("span-red span-green span-yellow").addClass("span-yellow");
        }
        else {
            $(this).removeClass("span-red span-green span-yellow").addClass("span-green");
        }
    });

}

function drawStars($__) {
    $inp = $__.find("input[checked=checked]");
    if ($inp.length == 0) {
        $__.find('label').removeClass("fa-star").addClass("fa-star-o");
    }
    else {
        $inp.first().prevAll("label").removeClass("fa-star-o").addClass("fa-star");
        $inp.first().nextAll("label").removeClass("fa-star").addClass("fa-star-o");
        $inp.first().next("label").removeClass("fa-star-o").addClass("fa-star");
    }
}

function reDraw(_div) {
    setRatingColor();

    if (_div == null) {
        $el = $("div.star-rating__wrap");
        $el.each(function (i, el) {
            drawStars($(this));
        });
    }
    else {
        drawStars(_div);
    }
}

function addMark(_form) {
    var $form = $(_form);
    var json_data = {
        Id: 0,
        MovieId: parseInt($form.find("input#Mark_MovieId").val()),
        UserProfileId: 0,
        Value: parseInt($form.find("input[checked=checked]").val()),
    };

    $.ajax({
        type: "POST",
        url: "/api/MarksApi",
        data: JSON.stringify(json_data),
        dataType: "json",
        contentType: "application/json",
        success: function (res) {
            $form.find("input#Mark_Id").attr("value", res.markId);
            $form.find("input#Mark_UserProfileId").attr("value", res.userProfileId);

            $("span.rating-details").html(res.rating);
            reDraw($form.closest("div"));
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function updateMark(_form) {
    var $form = $(_form);
    var json_data = {
        Id: parseInt($form.find("input#Mark_Id").val()),
        MovieId: parseInt($form.find("input#Mark_MovieId").val()),
        UserProfileId: parseInt($form.find("input#Mark_UserProfileId").val()),
        Value: parseInt($form.find("input[checked=checked]").val()),
    };

    $.ajax({
        type: "PUT",
        url: "/api/MarksApi/" + $form.find("input#Mark_Id").first().val(),
        data: JSON.stringify(json_data),
        dataType: "json",
        contentType: "application/json",
        success: function (res) {
            //$form.find("input#Mark_Id").removeAttr('value')
            //$form.find("input#Mark_UserProfileId").removeAttr('value');
            //$form.find("input[type=radio]").removeAttr("checked");

            $("span.rating-details").html(res.rating);
            reDraw($form.closest("div"));
        },
        error: function (e) {
            console.log(e);
        }
    });
}

function deleteMark(_form) {
    var $form = $(_form);

    $.ajax({
        type: "DELETE",
        url: "/api/MarksApi/" + $form.find("input#Mark_Id").first().val(),
        success: function (res) {
            $form.find("input#Mark_Id").removeAttr('value')
            $form.find("input#Mark_UserProfileId").removeAttr('value');
            $form.find("input[type=radio]").removeAttr("checked");
            $form.find("input[type=radio]").prop("checked", false);

            $("span.rating-details").html(res.rating);
            reDraw($form.closest("div"));
        }
    });
}

function getNotifications() {
    var res = "<ul class='list-group'>";
    $.ajax({
        url: "/Notifications/GetNotifications",
        method: "GET",
        success: function (result) {

            if (result.count != 0) {
                $("#notificationCount").html(result.count);
                $("#notificationCount").show('slow');
            } else {
                $("#notificationCount").html();
                $("#notificationCount").hide('slow');
                $("#notificationCount").popover('hide');
            }

            var notifications = result.userNotifications;
            notifications.forEach(element => {
                res = res + "<li class='list-group-item notification-text' "
                    +'data-id="' + element.id + '">'
                    + '<p class="notification-date">' + new Date(element.date).toLocaleString() + "</p>"
                    + '<p class="notification-text-p">' + element.text + "</p>" + "</li>";
            });

            res = res + "</ul>";

            $("#notification-content").html(res);

            console.log(result);
        },
        error: function (error) {
            console.log(error);
        }
    });
}

$(document).ready(function () {

    reDraw();

    $("#view-all").delegate(".wrapper-dropdown-3-search", "keyup", function (e) {
        if (e.keyCode == 13) {
            searchMovie();
        }
    });

    $("form").on("click", "i.cancel-rating__input", function () {
        var $inputs = $(this).closest("div").find("input");

        deleteMark($inputs.closest('form'));
    });

    $("form").on("change", "input.star-rating__input", function () {
        var $form = $(this.form);

        $(this).closest("div").find("input").removeAttr("checked");
        $(this).attr("checked", "checked");

        var markId = $form.find("input#Mark_Id").val();

        if (markId != undefined && markId != null && markId != '') {
            updateMark(this.form);
        }
        else {
            addMark(this.form);
        }
    });

    $("form label.star-rating__ico")
        .mouseover(function () {
            var $el = $(this);

            $el.removeClass("fa-star-o").addClass("fa-star");
            $el.prevAll("label").removeClass("fa-star-o").addClass("fa-star");
            $el.nextAll("label").removeClass("fa-star").addClass("fa-star-o");
        });

    $("form div.star-rating__wrap")
        .mouseleave(function () {
            reDraw($(this).closest("div"));
        });

    $("button.btn-fav").click(function () {
        var _btn = $(this);
        $.ajax({
            type: "POST",
            url: this.form.action,
            data: _btn.closest("form").serialize(),
            success: function (res) {
                if (_btn.html() == "Delete") {
                    _btn.closest("tr").remove();
                }
                else {
                    _btn.html(res.isFavourite);
                }
            },
            error: (e) => console.log(e),
        });
    });

    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({
        placement: 'bottom',
        content: function () {
            var _content = $("#notification-content").html();
            return _content;
        },
        html: true
    });

    $('#notificationCount').on('shown.bs.popover', function () {
        $("ul").on('click', 'li.notification-text', function () {
            var $current = $(this);
            var $__ = $('#notification-content').find('li.notification-text');
            var _id;
            $__.each(function (i, val) {
                if ($(val).find('p.notification-date').html() == $current.find('p.notification-date').html() &&
                    $(val).find('p.notification-text-p').html() == $current.find('p.notification-text-p').html()
                ) {
                    _id = $(val).data('id');
                }
            });

            $.ajax({
                type: "POST",
                url: "/Notifications/ReadNotification",
                data: { id: _id },
                success: function (res) {
                    $current.hide("slow");
                    getNotifications();
                },
                error: function (error) {
                    console.log(error);
                }

            });
        });

    });


    $('body').append(`<div id="notification-content" class="d-none"></div>`)

    getNotifications();

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/srServer")
        .build();

    connection.on("notifyUser", function () {
        getNotifications();
    });

    connection.start();


});