<template>
  <div>
    <h3>Games stats</h3>
    <v-container 
      fluid 
      grid-list-sm>
      <v-layout 
        v-for="(game, index) in gamesPlayed" 
        :key="index"
        row 
        wrap>
        <v-flex xs1>
          <span class="index">{{ index+1 }}.</span>
        </v-flex>
        <v-flex 
          xs11 
          class="u-ta-l">
          <v-chip
            v-for="(vote, index) in game" 
            :key="index"
            outline 
            color="green">
            <v-avatar class="green darken-4">{{ vote.message }}</v-avatar>
            <span v-if="players[vote.connectionId]">{{ players[vote.connectionId].Name }}</span>
            <span v-else>{{ vote.connectionId }}</span>
          </v-chip>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
/* eslint-disable-next-line no-unused-vars */
import Component from 'vue-class-component';

@Component({
  props: {
    gamesPlayed: Array,
    players: Object
  }
})
export default class GamesStats extends Vue {}
</script>
<style>
.index {
  font-size: 16px;
  font-weight: bold;
  line-height: 40px;
}
</style>
