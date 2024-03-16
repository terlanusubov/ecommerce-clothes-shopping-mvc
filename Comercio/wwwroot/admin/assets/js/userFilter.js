function Filter(callback, clickedPage) {
    const nameValue = document.getElementById("name").value;
    const surnameValue = document.getElementById("surname").value;
    const emailValue = document.getElementById("email").value;

    let xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            callback(this);

        }
    }

    xhr.open("GET", "https://localhost:7024/Admin/User/Filter?name=" + nameValue + "&surname=" + surnameValue + "&email=" + emailValue + "&page=" + clickedPage, true);

    xhr.send();
}

function FilterByPagination() {
    const paginations = document.querySelectorAll(".pagination a");

    for (let pagination of paginations) {
        if (typeof pagination.onclick == "function") {
            continue;
        }
        pagination.addEventListener("click", function (e) {
            e.preventDefault();

            const clickedPage = this.innerText;

            Filter((res) => {

                const previousActive = document.querySelector(".pagination li.active");

                previousActive.classList.remove("active");

                pagination.parentElement.classList.add("active");

                const result = JSON.parse(res.responseText);

                document.querySelector("#table-area").innerHTML = result.data;

                FilterByPagination();
                //TODO: status doesnt work

            }, clickedPage)



        })
       
    }
}

function ChangeUserStatus(callback) {
    const userStatuses = document.querySelectorAll(".user-status");

    for (let userStatus of userStatuses) {
        userStatus.addEventListener("click", function () {

            swal({
                title: 'İstifadəçinin statusunu dəyişdirmək istədiyinizə əminsinizmi?',
                text: "Status dəyişdirildikdən sonra bu istifadəçinin sayt üzrə hərəkətində müəyyən dəyişikliklər yaradacaqdır.",
                type: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yadda saxla',
                padding: '2em'
            }).then(function (result) {
                if (result.value) {

                    const changeStatusId = userStatus.getAttribute("data-change-id");

                    const userId = userStatus.getAttribute("data-userId");

                    const requestObj = {
                        userId: userId,
                        statusId: changeStatusId
                    };

                    let xhr = new XMLHttpRequest();

                    xhr.onreadystatechange = function () {
                        if (this.readyState == 4 && this.status == 200) {

                            callback(this, userStatus, changeStatusId);

                        }
                    }


                    xhr.open("POST", "https://localhost:7024/Admin/User/ChangeStatus", true);

                    xhr.setRequestHeader("content-type", "application/json");

                    xhr.send(JSON.stringify(requestObj));

                   
                }
            })


           

        });
    }
}