// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function sortByGenre(val) {
    
    $.ajax({
        type: "GET",
        url: "/Movies/IndexSort/?genreId=" + val,
        success: function (movies) {
            $("#view-all").html(movies)
        }
    });
}

function searchMovie () {
    search_value = $("#search_inp").val();
    $.ajax({
        type: "GET",
        url: "/Movies/IndexSearch?searchValue=" + search_value,
        success: function (movies) {
            $("#view-all").html(movies);
        }
    });
}

function inputSearch(e) {
    if (e.keyCode == 13) {
        searchMovie();
    }

}

//$(document).ready(function () {
//    $("div.div-m input.wrapper-dropdown-3-search").on("keyup", function (e) {
//        if (e.keyCode == 13) {
//            searchMovie();
//        }
//    });
//});