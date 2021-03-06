import React, { useState } from "react";
import StoryTable from "./StoryTable";
import "./css/AddStory.css";
import fetchData from "../../ApiCalls";
import { addStoryApi } from "../../ApiUrls";
import { getUserFromCookie } from "../../Utils";

export default function AddStory({ projectId, setIsAddStoryClicked, members}) {
  if (!getUserFromCookie()) {
    return <div>You need to sign in</div>;
  }
  const saveStory = async () => {
    const title = document.querySelector(".addStoryTitle")?.value;
    if (!title) {
      alert("title is required");
      return;
    }
    const type = parseInt(document.querySelector(".type")?.value ?? 0);
    const points = document.querySelector(".points")?.value;
    const ownerId = document.querySelector(".owner")?.value ?? 0;
    const response = await fetchData(addStoryApi, "POST", {
      type: type,
      projectId: projectId,
      points: points,
      ownerId: ownerId,
      title: title,
    });
    if (response?.id && response.id > 0) {
      alert("story created successfully");
    } 
		else if (response?.message) {
      alert(response.message);
    } 
		else {
      alert("error occured");
    }
  };
  
  const handleScreenClick = e => {
    e.target.classList[0] === "addStory" && setIsAddStoryClicked(false);
  }

  return (
    <div className="addStory" onClick={e => handleScreenClick(e)}>
      <div className="addStoryContainer">
        <button className="modalCloseButton" onClick={() => setIsAddStoryClicked(false)}>x</button>
        <textarea className="addStoryTitle" placeholder="title"></textarea>
        <StoryTable members={members} isAddStory={true}/>
        <textarea placeholder="description"></textarea>
        <button className="save" onClick={() => saveStory()}>
          Add Story
        </button>
      </div>
    </div>
  );
}
