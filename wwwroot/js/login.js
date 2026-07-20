const applicantTab = document.getElementById('applicantTab');
const adminTab = document.getElementById('adminTab');
const loginType = document.getElementById('loginType');
const loginTitle = document.getElementById('loginTitle');
const loginIntro = document.getElementById('loginIntro');
const userIdLabel = document.getElementById('userIdLabel');
const userId = document.getElementById('userId');

function selectLogin(type) {
    const admin = type === 'Admin';
    loginType.value = type;
    applicantTab.classList.toggle('active', !admin);
    adminTab.classList.toggle('active', admin);
    loginTitle.textContent = admin ? 'Admin / Registrar Login' : 'Applicant Login';
    loginIntro.textContent = admin
        ? 'Authorized KHB staff can securely sign in here.'
        : 'Sign in using your application number.';
    userIdLabel.textContent = admin ? 'Staff ID' : 'Application Number';
    userId.placeholder = admin ? 'e.g. REG-014' : 'e.g. APP202600123';
    userId.value = '';
}

applicantTab.addEventListener('click', () => selectLogin('Applicant'));
adminTab.addEventListener('click', () => selectLogin('Admin'));
selectLogin(loginType.value === 'Admin' ? 'Admin' : 'Applicant');
