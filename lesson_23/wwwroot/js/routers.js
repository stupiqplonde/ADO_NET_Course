document.addEventListener('DOMContentLoaded', () => {
    const forms = document.querySelectorAll('main form');
    const actions = [addGroup, addStudent, updateStudent, addTeacher, addSubject, addGrade, deleteStudent];
    forms.forEach((form, index) => {
        form.addEventListener('submit', event => {
            event.preventDefault();
            actions[index]();
        });
    });
    loadStudents();
});

function showMessage(text, error = false) {
    const message = document.getElementById('gr');
    message.textContent = text;
    message.className = error ? 'error' : 'success';
}

async function request(url, method = 'GET', data = null) {
    const options = { method: method };
    if (data !== null) {
        options.headers = { 'Content-Type': 'application/json' };
        options.body = JSON.stringify(data);
    }
    const response = await fetch(url, options);
    const text = await response.text();
    let result = null;
    try {
        result = text ? JSON.parse(text) : null;
    } catch {
        if (response.ok) throw new Error('Сервер вернул не JSON. Проверьте адрес запроса.');
    }
    if (!response.ok) {
        const description = result && (result.message || result.detail || result.title);
        throw new Error(description || `Ошибка запроса: ${response.status}`);
    }
    return result;
}

function addCell(row, value) {
    const cell = row.insertCell();
    cell.textContent = value;
}

function ClearPage() {
    document.getElementById('studentsTable').style.display = 'none';
    document.getElementById('groupsTable').style.display = 'none';
    document.getElementById('teachersTable').style.display = 'none';
    document.getElementById('subjectsTable').style.display = 'none';
    document.getElementById('gradesTable').style.display = 'none';
}

function formatDate(value) {
    if (!value) return '';
    return String(value).slice(0, 10);
}

async function loadStudents() {
    ClearPage();
    try {
        const students = await request('/students/all');
        if (!Array.isArray(students)) throw new Error('Сервер не вернул список студентов.');
        const body = document.getElementById('studentBody');
        body.replaceChildren();
        for (const student of students) {
            const row = body.insertRow();
            addCell(row, student.studentId);
            addCell(row, student.firstName);
            addCell(row, student.lastName);
            addCell(row, student.age);
            addCell(row, student.groupId);
        }
        document.getElementById('studentsTable').style.display = 'table';
        setActiveTab('students');
        return students;
    } catch (error) {
        showMessage(error.message, true);
        return null;
    }
}

async function loadGroups() {
    ClearPage();
    try {
        const groups = await request('/groups/all');
        if (!Array.isArray(groups)) throw new Error('Сервер не вернул список групп.');
        const body = document.getElementById('groupBody');
        body.replaceChildren();
        for (const group of groups) {
            const row = body.insertRow();
            addCell(row, group.groupId);
            addCell(row, group.groupName);
        }
        document.getElementById('groupsTable').style.display = 'table';
        setActiveTab('groups');
        return groups;
    } catch (error) {
        showMessage(error.message, true);
        return null;
    }
}

async function loadTeachers() {
    ClearPage();
    try {
        const teachers = await request('/teachers/all');
        if (!Array.isArray(teachers)) throw new Error('Сервер не вернул список преподавателей.');
        const body = document.getElementById('teacherBody');
        body.replaceChildren();
        for (const teacher of teachers) {
            const row = body.insertRow();
            addCell(row, teacher.teacherId);
            addCell(row, teacher.firstName);
            addCell(row, teacher.lastName);
        }
        document.getElementById('teachersTable').style.display = 'table';
        setActiveTab('teachers');
        return teachers;
    } catch (error) {
        showMessage(error.message, true);
        return null;
    }
}

async function loadSubjects() {
    ClearPage();
    try {
        const subjects = await request('/subjects/all');
        if (!Array.isArray(subjects)) throw new Error('Сервер не вернул список предметов.');
        const body = document.getElementById('subjectBody');
        body.replaceChildren();
        for (const subject of subjects) {
            const row = body.insertRow();
            addCell(row, subject.subjectId);
            addCell(row, subject.subjectName);
            addCell(row, subject.teacherId);
        }
        document.getElementById('subjectsTable').style.display = 'table';
        setActiveTab('subjects');
        return subjects;
    } catch (error) {
        showMessage(error.message, true);
        return null;
    }
}

async function loadGrades() {
    ClearPage();
    try {
        const grades = await request('/grades/all');
        if (!Array.isArray(grades)) throw new Error('Сервер не вернул список оценок.');
        const body = document.getElementById('gradeBody');
        body.replaceChildren();
        for (const grade of grades) {
            const row = body.insertRow();
            addCell(row, grade.gradeId);
            addCell(row, grade.studentId);
            addCell(row, grade.subjectId);
            addCell(row, grade.score);
            addCell(row, formatDate(grade.gradeDate));
        }
        document.getElementById('gradesTable').style.display = 'table';
        setActiveTab('grades');
        return grades;
    } catch (error) {
        showMessage(error.message, true);
        return null;
    }
}

function setActiveTab(tab) {
    const buttons = document.querySelectorAll('header button');
    buttons.forEach(button => {
        const active = button.dataset.tab === tab;
        button.classList.toggle('active', active);
        button.setAttribute('aria-pressed', active);
    });
}

function readStudent(editing = false) {
    const student = {
        firstName: document.getElementById(editing ? 'firstNameUpdate' : 'firstNameStudent').value.trim(),
        lastName: document.getElementById(editing ? 'lastNameUpdate' : 'lastNameStudent').value.trim(),
        age: Number(document.getElementById(editing ? 'ageUpdate' : 'ageStudent').value),
        groupId: Number(document.getElementById(editing ? 'groupIdUpdate' : 'groupId').value)
    };
    if (!student.firstName || student.firstName.length > 50 ||
        !student.lastName || student.lastName.length > 50) {
        throw new Error('Имя и фамилия должны содержать от 1 до 50 символов.');
    }
    if (!Number.isInteger(student.age) || student.age < 1 || student.age > 120) {
        throw new Error('Введите целый возраст от 1 до 120 лет.');
    }
    if (!Number.isInteger(student.groupId) || student.groupId < 1) {
        throw new Error('Введите положительный целый ID существующей группы.');
    }
    return student;
}

function readId(elementId, label = 'ID') {
    const id = Number(document.getElementById(elementId).value);
    if (!Number.isInteger(id) || id < 1) throw new Error(`Введите положительный целый ${label}.`);
    return id;
}

function setBusy(sectionIndex, busy) {
    const section = document.querySelectorAll('main section')[sectionIndex];
    section.querySelectorAll('button').forEach(button => button.disabled = busy);
}

async function addGroup() {
    setBusy(0, true);
    try {
        const name = document.getElementById('nameGroup').value.trim();
        if (!name || name.length > 20) throw new Error('Название группы: от 1 до 20 символов.');
        await request('/groups/add', 'POST', { groupName: name });
        document.getElementById('nameGroup').value = '';
        showMessage('Группа добавлена.');
        await loadGroups();
    } catch (error) {
        showMessage(error.message, true);
    } finally {
        setBusy(0, false);
    }
}

async function addStudent() {
    setBusy(1, true);
    try {
        const student = readStudent();
        const result = await request('/students/add', 'POST', student);
        document.getElementById('firstNameStudent').closest('form').reset();
        showMessage(`Студент добавлен. ID: ${result.studentId}`);
        await loadStudents();
    } catch (error) {
        showMessage(error.message, true);
    } finally {
        setBusy(1, false);
    }
}

async function updateStudent() {
    setBusy(2, true);
    try {
        const id = readId('idStudentUpdate', 'ID студента');
        const student = readStudent(true);
        await request(`/students/${id}`, 'PUT', student);
        const students = await loadStudents();
        if (students === null) return;
        const saved = students.find(s => s.studentId === id);
        if (!saved || saved.firstName !== student.firstName || saved.lastName !== student.lastName ||
            saved.age !== student.age || saved.groupId !== student.groupId) {
            throw new Error('Сервер принял запрос, но данные не изменились. Проверьте PUT в Program.cs.');
        }
        showMessage('Данные студента сохранены.');
    } catch (error) {
        showMessage(error.message, true);
    } finally {
        setBusy(2, false);
    }
}

async function addTeacher() {
    setBusy(3, true);
    try {
        const firstName = document.getElementById('firstNameTeacher').value.trim();
        const lastName = document.getElementById('lastNameTeacher').value.trim();
        if (!firstName || firstName.length > 50 || !lastName || lastName.length > 50) {
            throw new Error('Имя и фамилия преподавателя: от 1 до 50 символов.');
        }
        const result = await request('/teachers/add', 'POST', { firstName, lastName });
        document.getElementById('firstNameTeacher').closest('form').reset();
        showMessage(`Преподаватель добавлен. ID: ${result.teacherId}`);
        await loadTeachers();
    } catch (error) {
        showMessage(error.message, true);
    } finally {
        setBusy(3, false);
    }
}

async function addSubject() {
    setBusy(4, true);
    try {
        const subjectName = document.getElementById('subjectName').value.trim();
        const teacherId = readId('teacherId', 'ID преподавателя');
        if (!subjectName || subjectName.length > 100) {
            throw new Error('Название предмета: от 1 до 100 символов.');
        }
        const result = await request('/subjects/add', 'POST', { subjectName, teacherId });
        document.getElementById('subjectName').closest('form').reset();
        showMessage(`Предмет добавлен. ID: ${result.subjectId}`);
        await loadSubjects();
    } catch (error) {
        showMessage(error.message, true);
    } finally {
        setBusy(4, false);
    }
}

async function addGrade() {
    setBusy(5, true);
    try {
        const studentId = readId('gradeStudentId', 'ID студента');
        const subjectId = readId('gradeSubjectId', 'ID предмета');
        const score = Number(document.getElementById('gradeScore').value);
        const gradeDate = document.getElementById('gradeDate').value;
        if (!Number.isInteger(score) || score < 1 || score > 5) {
            throw new Error('Оценка должна быть целым числом от 1 до 5.');
        }
        if (!gradeDate) throw new Error('Укажите дату оценки.');
        const result = await request('/grades/add', 'POST', { studentId, subjectId, score, gradeDate });
        document.getElementById('gradeStudentId').closest('form').reset();
        showMessage(`Оценка добавлена. ID: ${result.gradeId}`);
        await loadGrades();
    } catch (error) {
        showMessage(error.message, true);
    } finally {
        setBusy(5, false);
    }
}

async function deleteStudent() {
    setBusy(6, true);
    try {
        const id = readId('idStudentDelete', 'ID студента');
        if (!confirm(`Удалить студента с ID ${id}?`)) return;
        await request(`/students/${id}`, 'DELETE');
        const students = await loadStudents();
        if (students === null) return;
        if (students.some(s => s.studentId === id)) {
            throw new Error('Сервер принял запрос, но студент не удалён. Проверьте DELETE в Program.cs.');
        }
        document.getElementById('idStudentDelete').value = '';
        showMessage('Студент удалён.');
    } catch (error) {
        showMessage(error.message, true);
    } finally {
        setBusy(6, false);
    }
}
