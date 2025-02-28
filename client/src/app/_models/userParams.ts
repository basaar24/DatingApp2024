import { User } from "./user";

export class UserParams {
    gender: string;
    minAge = 18;
    maxAge = 100;
    pageNumber = 1;
    pageSize = 12;

    constructor(user: User | null) {
        this.gender = user?.gender === "female" ? "male" : "female";
    }
}