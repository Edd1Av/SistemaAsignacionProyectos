export interface ILogin{
    email: string;
    password: string;
}

export interface IChangePassword{
    email: string;
    password: string;
    nPassword: string;
}

export interface IResetPassword{
    email: string;
}