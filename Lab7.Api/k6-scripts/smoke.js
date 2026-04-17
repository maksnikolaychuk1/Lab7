import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 1, // 1 віртуальний користувач
    duration: '1m', // Працює 1 хвилину
    thresholds: {
        http_req_duration: ['p(95)<200'], // 95% запитів < 200ms
        http_req_failed: ['rate==0'],     // 0% помилок
    },
};

export default function () {
    // Звертаємось до контейнера `api` з docker-compose по порту 8080
    const res = http.get('http://api:8080/api/students');

    check(res, {
        'status is 200': (r) => r.status === 200,
        'body size is large': (r) => r.body.length > 100000, // Перевірка, що прийшло багато даних
    });

    sleep(1); // Пауза між запитами
}