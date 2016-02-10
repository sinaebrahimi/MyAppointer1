$(document).ready(function () {
    $('#myModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var date = button.data('date') // Extract info from data-* attributes
        alert(date);
        var modal = $(this)
        modal.find('.modal-title').text('New Reservation for' + date)
        modal.find('.modal-body #BookDate').val(date)
    })


    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $('#submit-appointment').click(function () {
        var data = JSON.stringify($('form.reserve-form').serializeObject());
        $.ajax({
            type: "POST",
            url: "../api/Appointment",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if(msg == "overlap") {
                	alert('selected time have been reserved before');
                }else{
                	alert("new Appointment create successfully")
                }
            }
        });
    });

});