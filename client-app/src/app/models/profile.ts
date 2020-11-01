export interface IPhoto {
    id: string,
    url: string,
    isMain: boolean
}

export interface IProfile {
    displayName: string,
    userName: string,
    bio: string,
    image: string,
    photos: IPhoto[]
}