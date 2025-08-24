export class TestService {
    async callAPI(jwtToken: string) {
        if (!jwtToken) {
            console.error('No token available.');
            return;
        }

        const response = await fetch('http://localhost:5248/test/GetData', {
            method: 'GET',
            headers: {
                Authorization: `Bearer ${jwtToken}`,
            },
        });

        if (!response.ok) {
            console.error(`Failed to fetch data: ${response.statusText}`);
            return;
        }

        const data = await response.json();
        console.log("Data received:", data);
    }
}
