import * as _ from 'lodash'
export const getOnlineTicketCategoryOption = (ticketCategoryDetails) => {
  return ticketCategoryDetails.ticketCategoryDetails.map((item, index) => {
    let findTicketCat = _.find(ticketCategoryDetails.ticketCategories, { id: item.ticketCategoryId })
    let ticketCat = {
      label: findTicketCat.name,
      value: item.ticketCategoryId,
      description: item.description
    }
    return ticketCat
  })
}

export const getTicketDescription = (ticketCategoryId: any) => {
  if (ticketCategoryId == 1360) {
    return 'This allows you to view the experience/event'
  } else if (ticketCategoryId == 606) {
    return 'This access allows you to view the experience/event, and interact with the host/artist via chat. Please note, the host/artist will respond according to her/his schedule/program during the event/experience'
  } else if (ticketCategoryId == 19352 || ticketCategoryId == 12080) {
    return 'This access allows you to view the experience/event, and interact with the host/artist via chat and audio. Please note, the host/artist will respond according to her/his schedule/program during the event/experience'
  } else if (ticketCategoryId == 19350 || ticketCategoryId == 12079) {
    return 'This access allows you to view the experience/event, and interact with the host/artist via chat and video. Please note, the host/artist will respond according to her/his schedule/program during the event/experience'
  } else {
    return ''
  }
}

export const getTicketCategoryNotes = (ticketCategoryId: any) => {
  if (ticketCategoryId == 1360) {
    return ''
  } else if (ticketCategoryId == 606) {
    return ''
  } else if (ticketCategoryId == 19352 || ticketCategoryId == 12080) {
    return ''
  } else if (ticketCategoryId == 19350 || ticketCategoryId == 12079) {
    return '<li>Optimal internet speed is 50MBPS and higher</li><li>Backstage Pass will become active one minute prior before event time. Please click on Your Backstage Pass as soon as it becomes active</li><li>Once you click on the Backstage Pass, please mute yourself and also keep your video turned off from the controls on the Backstage Pass screen - this will enable an optimal performance to view the interaction</li><li> In case you cannot see the interaction, please re-click on the Backstage Pass button again</li><li>If you get a message popup after re-clicking on the Backstage Pass, please click ok and you should be able to get back in</li><li>Use the chat functionality to put your hand up and/or type a question for the host/s - the host/s will see it based on the volume of questions and respond in the best possible manner</li>'
  } else {
    return ''
  }
}
