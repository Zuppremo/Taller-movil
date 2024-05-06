export interface ILoginResponse {
    id: number;
    loginToken: string;
    status: string;
    responseData: {
        id: number;
        message: string;
    }
}