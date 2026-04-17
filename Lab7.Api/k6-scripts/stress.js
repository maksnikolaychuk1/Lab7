import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '2m', target: 10 },   // Базовий рівень
        { duration: '2m', target: 50 },   // Нормальне навантаження
        { duration: '2m', target: 100 },  // Наближення до меж
        { duration: '2m', target: 250 },  // Стрес
        { duration: '2m', target: 500 },  // Точка відмови (ймовірно)
    ],
};

export default function () {
    // Тестуємо важкий ендпоінт, щоб швидше знайти вузьке місце
    const res = http.get('http://api:8080/api/students/${randomId}');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    // Дуже коротка пауза для максимальної інтенсивності
    sleep(0.1);
}