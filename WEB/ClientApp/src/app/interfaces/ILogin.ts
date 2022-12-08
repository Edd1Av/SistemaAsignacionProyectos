export interface ILogin{
    email: string;
    password: string;
}

export interface IChangePassword{
    email: string;
    password: string;
    nPassword: string;
    nPasswordConfirm: string;
}

export interface IResetPassword{
    email: string;
}

export interface IPostAdmin{
    email: string;
}