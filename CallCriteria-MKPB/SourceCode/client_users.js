
$(function () {
    $('#add-report-popup').hide();
});


$(document).ready(function () {

    $(".EmptyData").parents("table").css("border-width", "0px").prop("border", "0");

    //return;
    var obj;

    //$('#ContentPlaceHolder1_gvMyUsers td:nth-child(2), #ContentPlaceHolder1_gvMyUsers td:nth-child(3), #ContentPlaceHolder1_gvMyUsers td:nth-child(4), #ContentPlaceHolder1_gvMyUsers td:nth-child(5), #ContentPlaceHolder1_gvMyUsers td:nth-child(7), #ContentPlaceHolder1_gvMyUsers td:nth-child(8)').css({ cursor: 'pointer' });
    //$('#ContentPlaceHolder1_gvMyUsers td:nth-child(3), #ContentPlaceHolder1_gvMyUsers td:nth-child(4), #ContentPlaceHolder1_gvMyUsers td:nth-child(5), #ContentPlaceHolder1_gvMyUsers td:nth-child(6), #ContentPlaceHolder1_gvMyUsers td:nth-child(8), #ContentPlaceHolder1_gvMyUsers td:nth-child(9)').addClass('editable');
    $('#edit-popup-done, #edit-popup-close, #edit-password-done, #edit-password-close').css({ cursor: 'pointer' });
    $('#edit-user-type-done, #edit-user-type-close, #edit-user-group-done, #edit-user-group-close').css({ cursor: 'pointer' });
    $('#edit-first-done, #edit-first-close, #edit-last-done, #edit-last-close').css({ cursor: 'pointer' });

    $('#emailadd').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#edit-popup-done').click();
            return false;
        }
    });

    //$('#first-name').keydown(function (e) {
    $('#first_name').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#edit-first-done').click();
            return false;
        }
    });

    //$('#last-name').keydown(function (e) {
    $('#last_name').keydown(function (e) {
        console.log('last-name-keydown');
        if (e.keyCode == 13) {
            $('#edit-last-done').click();
            return false;
        }
    });

    $('#new_password').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#edit-password-done').click();
            return false;
        }
    });

    $('#user_type').change(function (e) {
        $('#edit-user-type-done').click();
        return false;
    });

    $('#edit-user-group-content select').change(function (e) {
        $('#edit-user-group-done').click();
        return false;
    });

    //$('#ContentPlaceHolder1_gvMyUsers td:nth-child(4)').click(function () {
    /* 		$('#ContentPlaceHolder1_gvMyUsers td:nth-child(5)').click(function () {
                //console.log($(this).height() / 2)
                obj = $(this).parent();
				var editBox = $('#edit-password');
				var editInput = $('#new_password');
                $('#new_password').val($(this).text().trim());
                $('#new_password').select();
				editBox.css({ width: $(this).outerWidth()-1, height: $(this).outerHeight(),
								left: $(this).position().left+1, top: $(this).position().top,
								'background-color':$(this).parents('tr').css('background-color') });
				//editBox.find('.edit-content').css({ 'line-height':$(this).parents('tr').height()+'px' });
                $('#edit-password').show();
                $('#new_password').focus().blur(function() {
					//$('#edit-password-close').click();
					//editBox.find('.done-btn').click();
				});
            });

            //$('#ContentPlaceHolder1_gvMyUsers td:nth-child(5)').click(function () {
			$('#ContentPlaceHolder1_gvMyUsers td:nth-child(6)').click(function () {
                //console.log($(this).height() / 2)
                obj = $(this).parent();
				var editBox = $('#edit-popup');
				var editInput = $('#emailadd');
                $('#emailadd').val($(this).text().trim());
                $('#emailadd').select();
				editBox.css({ width: $(this).outerWidth()-1, height: $(this).outerHeight(),
								left: $(this).position().left+1, top: $(this).position().top,
								'background-color':$(this).parents('tr').css('background-color') });
				editBox.find('.edit-content').css({ 'line-height':$(this).parents('tr').height()+'px' });
                $('#edit-popup').show();
                $('#emailadd').focus().blur(function() {
					//$('#edit-popup-close').click();
					editBox.find('.done-btn').click();
				});
            });

            //$('#ContentPlaceHolder1_gvMyUsers td:nth-child(8)').click(function () {
			$('#ContentPlaceHolder1_gvMyUsers td:nth-child(9)').click(function () {
                //console.log($(this).height() / 2)
                obj = $(this).parent();
				var editBox = $('#edit-user-group');
				var editInput = $('#ContentPlaceHolder1_ddlUserGroup');
                $('#ContentPlaceHolder1_ddlUserGroup').val($(this).text().trim());
                $('#ContentPlaceHolder1_ddlUserGroup').select();
				editBox.css({ width: $(this).outerWidth()-1, height: $(this).outerHeight(),
								left: $(this).position().left+1, top: $(this).position().top,
								'background-color':$(this).parents('tr').css('background-color') });
				editBox.find('.edit-content').css({ 'line-height':$(this).parents('tr').height()+'px' });
                $('#edit-user-group').show();
                $('#ContentPlaceHolder1_ddlUserGroup').focus().blur(function() {
					//$('#edit-user-group-close').click();
					editBox.find('.done-btn').click();
				});
            });

            //$('#ContentPlaceHolder1_gvMyUsers td:nth-child(7)').click(function () {
			$('#ContentPlaceHolder1_gvMyUsers td:nth-child(8)').click(function () {
                //console.log($(this).height() / 2)
                obj = $(this).parent();
				var editBox = $('#edit-user-type');
				var editInput = $('#user_type');
                //$('#user_type').find('option[value="' + $(this).text() + '"]').attr();
                $('#user_type').val($(this).text().trim());
                $('#user_type').select();
				editBox.css({ width: $(this).outerWidth()-1, height: $(this).outerHeight(),
								left: $(this).position().left+1, top: $(this).position().top,
								'background-color':$(this).parents('tr').css('background-color') });
				editBox.find('.edit-content').css({ 'line-height':$(this).parents('tr').height()+'px' });
                $('#edit-user-type').show();
                $('#user_type').focus().blur(function() {
					//$('#edit-user-type-close').click();
					editBox.find('.done-btn').click();
				});
            });


            //$('#ContentPlaceHolder1_gvMyUsers td:nth-child(2)').click(function () {
			$('#ContentPlaceHolder1_gvMyUsers td:nth-child(3)').click(function () {
                //console.log($(this).height() / 2)
				obj = $(this).parent();
				var editBox = $('#edit-first');
				var editInput = $('#first_name');
				$('#first_name').val($(this).text().trim());
				$('#first_name').select();
				editBox.css({ width: $(this).outerWidth()-1, height: $(this).outerHeight(),
								left: $(this).position().left+1, top: $(this).position().top,
								'background-color':$(this).parents('tr').css('background-color') });
				editBox.find('.edit-content').css({ 'line-height':$(this).parents('tr').height()+'px' });
				$('#edit-first').show();
				$('#first_name').focus().blur(function() {
					//$('#edit-first-close').click();
					editBox.find('.done-btn').click();
				});
            });



			//$('#ContentPlaceHolder1_gvMyUsers td:nth-child(3)').click(function () {
			$('#ContentPlaceHolder1_gvMyUsers td:nth-child(4)').click(function () {
                //console.log($(this).height() / 2)
                obj = $(this).parent();
				var editBox = $('#edit-last');
				var editInput = $('#last_name');
                $('#last_name').val($(this).text().trim());
                $('#last_name').select();
				editBox.css({ width: $(this).outerWidth()-1, height: $(this).outerHeight(),
								left: $(this).position().left+1, top: $(this).position().top,
								'background-color':$(this).parents('tr').css('background-color') });
				editBox.find('.edit-content').css({ 'line-height':$(this).parents('tr').height()+'px' });
                $('#edit-last').show();
                $('#last_name').focus().blur(function() {
					//$('#edit-last-close').click();
					editBox.find('.done-btn').click();
				});
            }); */

    $('#edit-password-done').click(function () {
        //console.log(obj.find('td:nth-child(4)').text())
        if ($('#new_password').val().length < 6) {
            alert('Password must be at least 6 characters.');
        }
        else {
            if (obj != null) {
                $.ajax({
                    type: "POST",
                    url: "client_users.aspx/UpdateUserPassword",
                    data: '{"username" : "' + obj.find('td:nth-child(2)').text() + '","newpassword":"' + $('#new_password').val() + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function () {
                        obj.find('td:nth-child(5)').text('******');
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
                $('#edit-password').hide();
            }
        }
    });

    $('#edit-popup-done').click(function () {
        //console.log(obj.find('td:nth-child(4)').text())
        if (obj != null) {
            $.ajax({
                type: "POST",
                url: "client_users.aspx/UpdateUserEmail",
                data: '{"username" : "' + obj.find('td:nth-child(2)').text() + '","oldemail":"' + obj.find('td:nth-child(5)').text() + '","newemail":"' + $('#emailadd').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    obj.find('td:nth-child(6)').text($('#emailadd').val());
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
            $('#edit-popup').hide();
        }
    });


    $('#edit-first-done').click(function () {
        //console.log(obj.find('td:nth-child(4)').text())
        if (obj != null) {
            $.ajax({
                type: "POST",
                url: "client_users.aspx/UpdateUserFirst",
                data: '{"username" : "' + obj.find('td:nth-child(2)').text() + '","type":"' + $('#first_name').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    obj.find('td:nth-child(3)').text($('#first_name').val());
                },
                failure: function (response) {
                    console.log('fail');
                    alert(response.d);
                }
            });
            $('#edit-first').hide();
        }
    });


    $('#edit-last-done').click(function () {
        //console.log(obj.find('td:nth-child(4)').text())
        if (obj != null) {
            $.ajax({
                type: "POST",
                url: "client_users.aspx/UpdateUserLast",
                data: '{"username" : "' + obj.find('td:nth-child(2)').text() + '","type":"' + $('#last_name').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    obj.find('td:nth-child(4)').text($('#last_name').val());
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
            $('#edit-last').hide();
        }
    });





    $('#edit-user-group-done').click(function () {
        //console.log(obj.find('td:nth-child(4)').text())
        if (obj != null) {
            $.ajax({
                type: "POST",
                url: "client_users.aspx/UpdateUserGroup",
                data: '{"username" : "' + obj.find('td:nth-child(2)').text() + '","group":"' + $('#ContentPlaceHolder1_ddlUserGroup option:selected').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    obj.find('td:nth-child(9)').text($('#ContentPlaceHolder1_ddlUserGroup').val());
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
            $('#edit-user-group').hide();
        }
    });

    $('#edit-user-type-done').click(function () {
        //console.log(obj.find('td:nth-child(4)').text())
        if (obj != null) {
            $.ajax({
                type: "POST",
                url: "client_users.aspx/UpdateUserType",
                data: '{"username" : "' + obj.find('td:nth-child(2)').text() + '","type":"' + $('#user_type option:selected').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    obj.find('td:nth-child(8)').text($('#user_type').val());
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
            $('#edit-user-type').hide();
        }
    });



    $('#edit-password-close').click(function () {
        $('#edit-password').hide();
    });

    $('#edit-user-group-close').click(function () {
        $('#edit-user-group').hide();
    });

    $('#edit-user-type-close').click(function () {
        $('#edit-user-type').hide();
    });


    $('#edit-first-close').click(function () {
        $('#edit-first').hide();
    });

    $('#edit-last-close').click(function () {
        $('#edit-last').hide();
    });

    $('#edit-popup-close').click(function () {
        $('#edit-popup').hide();
    });

    $('input:checkbox[id*="chkActivate"]').change(function () {
        obj = $(this).parent();
        var isChecked = $(this).is(":checked");
        var el = $(this);
        el.hide();
        //console.log(obj.parent().find('td:nth-child(4)').text() + '-' + $(this).is(":checked"))
        if (obj != null) {
            $.ajax({
                type: "POST",
                url: "CDService.svc/UpdateUserActive",
                data: '{"username" : "' + obj.parent().find('td:nth-child(1)').text() + '","isActive":"' + isChecked + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    var role = '';
                    if (isChecked)
                        role = 'QA';
                    else
                        role = 'Inactive';
                    obj.parent().find('[id*=ddlUserType] option').filter(function () { return $(this).html() == role; }).attr('selected', 'selected');
                    el.show();
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
    });

    //$('.dash-modules .detailsTable input[type="checkbox"]').after('<span class="checkDisplay"></span>');
    //$('input[type="checkbox"]').after('<span class="checkDisplay"></span>');
    //$('.checkDisplay').click(function () {
    //    var checkbox = $(this).prev('input[type="checkbox"]');
    //    if (!checkbox.attr('disabled')) {
    //        checkbox.prop('checked', !checkbox.prop('checked'));
    //    }
    //});




    $('input[name="filter-textbox"]').keydown(function (e) {
        if (e.keyCode === 27) {
            $(this).val('');
        }
    });


    $('input[name="filter-textbox"]').keyup(function () {
        var filterText = $(this).val().trim();
        if (filterText == '') {
            $('.detailsTable tr').removeClass('hide');
        } else {
            $('.detailsTable tr').not(':first').each(function () {
                var rowContains = $(this).text().toLowerCase().indexOf(filterText.toLowerCase()) > -1;
                if (rowContains) {
                    $(this).removeClass('hide');
                } else {
                    $(this).addClass('hide');
                }
            });
        }
    });



    $('input[name="filter-inactive"]').change(function (e) {
        var this_box = this;
        if ($(this).is(':checked')) {
            $('.detailsTable tr').not(':first').each(function () {
                var rowContains = $(this).text().toLowerCase().indexOf('inactive') > -1;
                if (rowContains) {
                    $(this).addClass('hide');
                }
            });
        }

        else {
            $('.detailsTable tr').not(':first').each(function () {
                var rowContains = $(this).text().toLowerCase().indexOf('inactive') > -1;
                if (rowContains) {
                    $(this).removeClass('hide');
                }
            });
        }


    });




    $('.add-user-btn').click(function () {
        $('#add-user-popup-bg').addClass('show');
    });
    $('.popup2-background').click(function () {
        $(this).removeClass('show');
    });
    $('.close-popup2').click(function () {
        $(this).parents('.popup2').prev('.popup2-background').removeClass('show');
    });
    $('.popup2 input').keyup(function () {
        checkAddUser();
    });
    $('.popup2 input').change(function () {
        checkAddUser();
    });
    $('.popup2 .checkDisplay').click(function () {
        checkAddUser();
    });

});


function checkAddUser() {
    var enabledBtn = true;
    $('.popup2 .required-field').each(function () {
        if ($(this).val().trim() == '') {
            enabledBtn = false;
        }
    });
    $('.popup2 .required-checks').each(function () {
        var tempEnabled = false;
        $(this).find('input[type="checkbox"]').each(function () {
            if ($(this).prop('checked') == true) {
                tempEnabled = true;
            }
        });
        if (tempEnabled == false) {
            enabledBtn = false;
        }
    });
    if (enabledBtn == true) {
        $('.popup2 .add-user-submit').removeAttr('disabled');
    } else {
        $('.popup2 .add-user-submit').attr('disabled', 'disabled');
    }
}

function openEditUserPopup(userid) {
    $('#edit-user-popup-bg').addClass('show');
    $('#edit-user-popup iframe').removeClass('show')
								.attr('src', 'edit_client_user.aspx?user=' + userid);
    $('#edit-user-popup iframe').on('load', function () {
        $(this).addClass('show');
    });
}
function closeEditUserPopup() {
    $('#edit-user-popup-bg').removeClass('show');
    //location.reload();
}
















