function getDevelopersOnProject() {
    var id = document.getElementById('projectId').value;
    let developer = document.getElementById("DeveloperId");
    if (id == '') {
        developer.innerHTML = '<option value="">Choose Developer</option>';
    } else {
        developer.innerHTML = '';
    }
    var url = "/Home/DevelopersOnProject/" + id;
    $.post(url).then(function (respone) {
        var dropDown = document.getElementById('DeveloperId');

       
        for (let i = 0; i < respone.fullName.length; i++) {
            var option = document.createElement('option');
            let devId = respone.ids[i];
            let devName = respone.fullName[i];
            option.value = devId;
            option.text = devName;
            dropDown.add(option);
        }
    });
}
(function ($) {
    "use strict";

    var nav = $('nav');
    var navHeight = nav.outerHeight();

    $('.navbar-toggler').on('click', function () {
        if (!$('#mainNav').hasClass('navbar-reduce')) {
            $('#mainNav').addClass('navbar-reduce');
        }
    })

    // Preloader
    $(window).on('load', function () {
        if ($('#preloader').length) {
            $('#preloader').delay(10).fadeOut('slow', function () {
                $(this).remove();
            });
        }
    });

    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 1500, 'easeInOutExpo');
        return false;
    });
    // Closes responsive menu when a scroll trigger link is clicked
    $('.js-scroll').on("click", function () {
        $('.navbar-collapse').collapse('hide');
    });
    // Activate scrollspy to add active class to navbar items on scroll
    $('body').scrollspy({
        target: '#mainNav',
        offset: navHeight
    });
    /*--/ End Scrolling nav /--*/
})(jQuery);


function display_c() {
    var refresh = 1000; // Refresh rate in milli seconds
    mytime = setTimeout('display_ct()', refresh)
}

function display_ct() {
    document.getElementById('gsc-i-id1').placeholder = 'Search...';
    //$("#gsc-i-id1").addClass("form-control form-control-navbar");
    var x = new Date()
    let options = {
        hour: "2-digit", minute: "2-digit",
        second: "2-digit", weekday: "long", year: "numeric", month: "short",
        day: "numeric"
    };
    document.getElementById('ct').innerText = x.toLocaleTimeString("en-us", options);
    display_c();  
}




 $(function () {
     oTable = $("#example1,#homeTable,#notificationTable,#viewTicket,#viewProject,#viewCompany,#tickethistory,#useroverview,#viewUserRole").DataTable({
      "responsive": true,
      "autoWidth": false,
        "paging": true,
         "searching": true,    
    });
    $('#example2').DataTable({
      "paging": true,
      "lengthChange": false,
      "searching": false,
      "ordering": true,
      "info": true,
      "autoWidth": false,
      "responsive": true,
    });

  
  });
$('#myInputTextField,#myInputHomeField,#inputNotification,#searchTicket,#searchProject,#searchCompany,#searchTicketHistory,#searchUser,#searchUserRole').keyup(function () {
    oTable.search($(this).val()).draw();
});


$('#contact,#Message').summernote({
    height: 300,
    placeholder: "Tell me, I'm listening...",
    tabDisable: true,
    dialogsInBody: true
});
$('#Content').summernote({
    height: 120,
    placeholder: "Tell me, I'm listening...",
    tabDisable: true,
    dialogsInBody: true
});
$(function () {
    //Initialize Select2 Elements
    $('.select2').select2()
    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    })
    //Bootstrap Duallistbox
    $('.duallistbox').bootstrapDualListbox()
})


