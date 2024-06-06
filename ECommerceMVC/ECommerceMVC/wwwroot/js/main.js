import { apiGetCurrentUser } from "./appApi/user.js"
import managerUser from "./adminPages/managerUsers.js"


document.addEventListener("DOMContentLoaded", async () => {

    try {
        const res = await apiGetCurrentUser()
        if (res && res?.status) {
            console.log(res?.data)
        } else {
            window.location.href = "/Account/Login"
        }
    } catch (e) {
        console.error(e)
        window.location.href = "/Account/Login"
    }
    // Lấy URL hiện tại
    var currentUrl = window.location.href;

    // Tách URL thành các thành phần
    var urlParts = currentUrl.split("/");

    // Lấy phần tử thứ ba của URL để kiểm tra
    var targetPart = urlParts[3];

    console.log(urlParts)

    // Sử dụng câu lệnh switch case để kiểm tra phần cuối của URL
    switch (targetPart) {
        case "Admin":
            // Thực hiện các hành động cho trang Admin
            console.log("Bạn đang ở trang Admin");
            managerUser.init()
            break;
        case "Member":
            // Thực hiện các hành động cho trang Member
            console.log("Bạn đang ở trang Member");
            break;
        default:
            // Thực hiện các hành động mặc định
            console.log("Bạn đang ở trang khác");
            break;
    }
   
})