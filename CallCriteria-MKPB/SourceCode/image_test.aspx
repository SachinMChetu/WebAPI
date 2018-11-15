<%@ Page Title="" Language="VB" MasterPageFile="~/CC_Master.master" AutoEventWireup="false" CodeFile="image_test.aspx.vb" Inherits="image_test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        canvas#viewport {
            border: 1px solid white;
            width: 900px;
        }

          .rectangle {
        border: 1px solid #FF0000;
        position: absolute;
    }
    </style>

    <canvas id="viewport"></canvas>

    <script>
        document.onkeydown = function (e) { return on_keyboard_action(e); }
        document.onkeyup = function (e) { return on_keyboardup_action(e); }

        var canvas = document.getElementById("viewport");
        var ctx = canvas.getContext("2d");
        var ctrl_pressed = false;

        function on_keyboard_action(event) {
            k = event.keyCode;
            //ctrl
            if (k == 17) {
                if (ctrl_pressed == false)
                    ctrl_pressed = true;
                if (!window.Clipboard)
                    pasteCatcher.focus();
            }
        }
        function on_keyboardup_action(event) {
            //ctrl
            if (k == 17)
                ctrl_pressed = false;
        }


        //=== Clipboard ================================================================

        //firefox
        var pasteCatcher;
        if (!window.Clipboard) {
            pasteCatcher = document.createElement("div");
            pasteCatcher.setAttribute("id", "paste_ff");
            pasteCatcher.setAttribute("contenteditable", "");
            pasteCatcher.style.cssText = 'opacity:0;position:fixed;top:0px;left:0px;';
            pasteCatcher.style.marginLeft = "-20px";
            document.body.appendChild(pasteCatcher);
            pasteCatcher.focus();
            document.addEventListener("click", function () {
                //pasteCatcher.focus();
            });
            document.getElementById('paste_ff').addEventListener('DOMSubtreeModified', function () {
                if (pasteCatcher.children.length == 1) {
                    img = pasteCatcher.firstElementChild.src;

                    var img2 = new Image();
                    img2.onload = function () {
                        ctx.drawImage(img2, 0, 0);
                    }
                    img2.src = img;
                    //ctx.drawImage(img, 0, 0);


                    //ctx.drawImage(img, 0, 0);
                    pasteCatcher.innerHTML = '';
                }
            }, false);
        }
        //chrome
        window.addEventListener("paste", pasteHandler);
        function pasteHandler(e) {
            if (e.clipboardData) {
                var items = e.clipboardData.items;
                if (items) {
                    for (var i = 0; i < items.length; i++) {
                        if (items[i].type.indexOf("image") !== -1) {
                            var blob = items[i].getAsFile();
                            var URLObj = window.URL || window.webkitURL;
                            var source = URLObj.createObjectURL(blob);
                            paste_createImage(source);
                        }
                    }
                }
                    // If we can't handle clipboard data directly (Firefox),
                    // we need to read what was pasted from the contenteditable element
                else {
                }
            }
            else {
                setTimeout(paste_check_Input, 1);
            }
        }
        function paste_check_Input() {
            var child = pasteCatcher.childNodes[0];
            pasteCatcher.innerHTML = "";
            if (child) {
                if (cild.tagName === "IMG") {
                    paste_createImage(child.src);
                }
            }
        }
        function paste_createImage(source) {
            var pastedImage = new Image();
            pastedImage.onload = function () {
                ctx.drawImage(pastedImage, 0, 0);
            }
            pastedImage.src = source;
        }

        //=== /Clipboard ===============================================================


        function initDraw(canvas) {
            function setMousePosition(e) {
                var ev = e || window.event; //Moz || IE
                if (ev.pageX) { //Moz
                    mouse.x = ev.pageX + window.pageXOffset;
                    mouse.y = ev.pageY + window.pageYOffset;
                } else if (ev.clientX) { //IE
                    mouse.x = ev.clientX + document.body.scrollLeft;
                    mouse.y = ev.clientY + document.body.scrollTop;
                }
            };

            var mouse = {
                x: 0,
                y: 0,
                startX: 0,
                startY: 0
            };
            var element = null;

            canvas.onmousemove = function (e) {
                setMousePosition();
                if (element !== null) {
                    element.style.width = Math.abs(mouse.x - mouse.startX) + 'px';
                    element.style.height = Math.abs(mouse.y - mouse.startY) + 'px';
                    element.style.left = (mouse.x - mouse.startX < 0) ? mouse.x + 'px' : mouse.startX + 'px';
                    element.style.top = (mouse.y - mouse.startY < 0) ? mouse.y + 'px' : mouse.startY + 'px';
                }
            }

            canvas.onclick = function (e) {
                if (element !== null) {
                    element = null;
                    canvas.style.cursor = "default";
                    console.log("finsihed.");
                } else {
                    console.log("begun.");
                    mouse.startX = mouse.x;
                    mouse.startY = mouse.y;
                    element = document.createElement('div');
                    element.className = 'rectangle'
                    element.style.left = mouse.x + 'px';
                    element.style.top = mouse.y + 'px';
                    canvas.appendChild(element)
                    canvas.style.cursor = "crosshair";
                }
            }
        }


        initDraw(document.getElementById("viewport"));


    </script>

</asp:Content>

