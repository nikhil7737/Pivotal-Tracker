const apiHostUrl = "http://localhost:5000/api/";
export const allProjectsApi = `${apiHostUrl}get/allprojects`;
export const getProjectApi = id => `${apiHostUrl}get/project/${id}`;
export const getStoryApi = id => `${apiHostUrl}get/story/${id}`;
export const logoutUrl = `${apiHostUrl}logout`;
export const loginUrl = `${apiHostUrl}login`;
export const signupUrl = `${apiHostUrl}signup`;


//comments
export const postCommentApi = `${apiHostUrl}create/comment`;
export const updateCommentApi = `${apiHostUrl}update/comment`;
export const deleteCommentApi = `${apiHostUrl}delete/comment`;
export const getCommentsApi = id => `${apiHostUrl}get/comment/${id}`;
