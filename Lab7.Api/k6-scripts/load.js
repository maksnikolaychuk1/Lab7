import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 50 },  // Плавне наростання до 50 VU
        { duration: '4m', target: 50 },   // Утримання навантаження 4 хвилини
        { duration: '30s', target: 0 },   // Плавне зниження
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'],
        http_req_failed: ['rate<0.01'],   // Допускаємо менше 1% помилок
    },
};

export default function () {
    const res = http.get('http://api:8080/api/students');

    check(res, {
        'status is 200': (r) => r.status === 200,
        'response time < 500ms': (r) => r.timings.duration < 500,
    });

    sleep(1);
}