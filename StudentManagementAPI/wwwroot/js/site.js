// فایل JavaScript برای سیستم مدیریت دانشجویان

// اجرای کد پس از بارگذاری کامل صفحه
$(document).ready(function () {
    console.log('سیستم مدیریت دانشجویان آماده است!');
    
    // اضافه کردن کلاس active به لینک منوی فعلی
    const currentUrl = window.location.pathname;
    $('.navbar-nav .nav-link').each(function () {
        const linkUrl = $(this).attr('href');
        if (currentUrl.includes(linkUrl) && linkUrl !== '/') {
            $(this).addClass('active');
        } else if (currentUrl === '/' && linkUrl === '/') {
            $(this).addClass('active');
        }
    });
    
    // تأیید حذف دانشجو
    $('.delete-confirm').on('click', function (e) {
        if (!confirm('آیا از حذف این دانشجو اطمینان دارید؟')) {
            e.preventDefault();
        }
    });
});