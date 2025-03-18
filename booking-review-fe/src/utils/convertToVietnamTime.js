export const convertToVietnamTime = (utcDateString) => {
    if (!utcDateString) return "";
  
    const utcDate = new Date(utcDateString);
    if (isNaN(utcDate.getTime())) return "Invalid Date";
  
    // convert UTC date to Vietnam date
    const vietnamOffset = 7 * 60 * 60 * 1000;
    const vietnamTime = new Date(utcDate.getTime() + vietnamOffset);
  
    // get current date & time
    const now = new Date();
    const vietnamNow = new Date(now.getTime() + vietnamOffset);
  
    // calculate days difference
    const daysDiff =
      Math.floor(vietnamNow.getTime() / (1000 * 60 * 60 * 24)) -
      Math.floor(vietnamTime.getTime() / (1000 * 60 * 60 * 24));
  
    // get time string
    const timeString = vietnamTime.toLocaleTimeString("vi-VN", {
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });
  
    if (daysDiff === 0) return `Today, ${timeString}`;
    if (daysDiff === 1) return `Yesterday, ${timeString}`;
    if (daysDiff > 1 && daysDiff <= 5) return `${daysDiff} days ago, ${timeString}`;
  
    return vietnamTime.toLocaleString("vi-VN", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });
  };
  