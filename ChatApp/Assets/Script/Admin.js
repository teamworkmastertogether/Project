$(document).ready(function () {
    $('#btnClickAdd').click(function () {
        $('#add-user').find('#userName').val('');
        $('#add-user').find('#userEmail').val('');
        $('.input2').removeClass('border-red');
    });

    $('.btn-create-user').on('click', function () {
        var check = true;
        $('.input2').removeClass('border-red');
        $('.input2').each(function () {
            if ($(this).val() === "" || $(this).val().length < 5 || $(this).val().length > 200) {
                $(this).addClass('border-red');
                check = false;
                $(this).tooltip("show");
            }
        });

        if (!isStudentEmail($("#userEmail").val().trim())) {
            $('input[name="Email"]').tooltip("show");
            $('#userEmail[data-toggle="tooltip"]').addClass('border-red');
            check = false;
        }

        if (check) {
            var dataEmail = {
                Email: $('#add-user').find('#userEmail').val()
            };
            $.ajax({
                type: "POST",
                url: "/Admin/CheckEmail",
                data: JSON.stringify(dataEmail),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                cache: false,
                success: function (result) {
                    if (result) {
                        $('input[name="Email"]').tooltip("show");
                        check = false;
                    }
                }, error: function (message) {
                    alert(message.responseText);
                }
            });
        }

        setTimeout(function () {
            if (check) {
                $('#myModal').modal('hide');
                Swal.fire(
                    'Thành công!',
                    'Bạn đã thêm thành viên thành công!',
                    'success'
                );
                var dataObj = {
                    Name: $('#add-user').find('#userName').val(),
                    Email: $('#add-user').find('#userEmail').val()
                };
                console.log(dataObj);
                $('#add-user').find('#userName').val('');
                $('#add-user').find('#userEmail').val('');
                $.ajax({
                    type: "POST",
                    url: "/Admin/CreateUser",
                    data: JSON.stringify(dataObj),
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (result) {
                    }, error: function (message) {
                        alert(message.responseText);
                    }
                });
            } 
        }, 700);
    });

    $('#userName').blur(function () {
        $(this).val(ChuanhoaTen($(this).val()));
    });

    function ChuanhoaTen(ten) {
        dname = ten;
        ss = dname.split(' ');
        dname = "";
        for (i = 0; i < ss.length; i++)
            if (ss[i].length > 0) {
                if (dname.length > 0) dname = dname + " ";
                dname = dname + ss[i].substring(0, 1).toUpperCase();
                dname = dname + ss[i].substring(1).toLowerCase();
            }
        return dname;
    }

    // Kiểm tra email
    function isStudentEmail(e) {
        var regex = /(^[\w.+\-]+@gmail\.com$)/;
        if (regex.test(e)) return true;
        return false;
    }

    $('.input2').blur(function () {
        if ($(this).val() !== "") {
            $(this).removeClass('border-red');
        } else {
            $(this).addClass('border-red');
        }
    });

    $('.btn-disable-user').on('click', function () {
        var id = parseInt($(this).attr('id'));
        var button = this;
        $.ajax({
            type: "POST",
            url: "/Admin/LockUser/?id=" + id,
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result === 1) {
                    Swal.fire(
                        'Thành công!',
                        'Bạn đã khóa tài khoản này thành công!',
                        'success'
                    );
                    $(button).text("Mở khóa");
                    $(button).removeClass('btn-danger');
                    $(button).addClass('btn-success');
                }
                else if (result === 0) {
                    Swal.fire(
                        'Thành công!',
                        'Bạn đã mở khóa tài khoản này thành công!',
                        'success'
                    );
                    $(button).text("Khóa");
                    $(button).removeClass('btn-success');
                    $(button).addClass('btn-danger');
                }
            }, error: function (message) {
                alert(message.responseText);
            }
        });
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#blah').attr('src', e.target.result);
            }
            $("#upImg").show();
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#UploadImage").change(function () {
        readURL(this);
    });

    //đổi nút chọn file sang button avatar
    $("#selectFile").click(function () {
        $("#UploadImage").trigger('click');
    });

    if ($('.show-result').text().trim() !== "") {
        var successImported = $('.show-result').text().trim() + ' người được import thành công';
        Swal.fire(
            'Thành công',
            successImported,
            'success'
        );
        $('.show-result').text("");
    }

    if ($('.import-fail').text().trim() !== "") {
        var failImported = $('.import-fail').text().trim(); 
        Swal.fire(
            'Thất bại',
            failImported,
            'error'
        );
    }
});

var idCurrentImg;

$('.btn-change-cover').on('click', function () {
    idCurrentImg = $(this).attr('id');
    var imgSrc = $(this).closest('td').prev().find('img').attr('src');
    $(this).parents('.content-wrapper').next().find('#blah').attr('src', imgSrc);
});

function UploadSubjectPhoto(formData) {
    url = "/Admin/UploadCover?id=" + idCurrentImg;
    
    var ajaxConfig = {
        type: "POST",
        url: url,
        data: new FormData(formData),
        success: function (result) {
            $('.photo-cover-' + idCurrentImg).attr('src', result.Photo);
            $("#upImg").hide();
            $(".modal-backdrop").remove();
            $("#myModal #close").click();
            Swal.fire(
                'Thành công!',
                'Bạn đã thay đổi ảnh thành công!',
                'success'
            );
        }
    };
    if ($(formData).attr('enctype') === "multipart/form-data") {
        ajaxConfig["contentType"] = false;
        ajaxConfig["processData"] = false;
    }

    $.ajax(ajaxConfig);

    return false;
}

